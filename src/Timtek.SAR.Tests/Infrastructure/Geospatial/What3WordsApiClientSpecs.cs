using System.Net;
using System.Text.Json;
using Timtek.SAR.Application.Geospatial;
using Timtek.SAR.Domain.ValueObjects;
using Timtek.SAR.Infrastructure.Geospatial;

namespace Timtek.SAR.Tests.Infrastructure.Geospatial;

class What3WordsApiClientContextBuilder
{
    readonly List<(Func<HttpRequestMessage, bool> predicate, HttpResponseMessage response)> _responses = [];
    string _apiKey = "test-api-key";

    public What3WordsApiClientContextBuilder WithApiKey(string key)
    {
        _apiKey = key;
        return this;
    }

    public What3WordsApiClientContextBuilder WithConvertToAddressResponse(double lat, double lng, string words)
    {
        var json = JsonSerializer.Serialize(new
        {
            coordinates = new { lat, lng },
            words,
            country = "GB",
            nearestPlace = "Test Place",
            language = "en",
            map = $"https://w3w.co/{words}"
        });

        _responses.Add((req =>
            req.RequestUri!.AbsolutePath.Contains("convert-to-3wa") &&
            req.RequestUri.Query.Contains($"coordinates={lat}"),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            }));
        return this;
    }

    public What3WordsApiClientContextBuilder WithConvertToCoordinatesResponse(string words, double lat, double lng)
    {
        var json = JsonSerializer.Serialize(new
        {
            coordinates = new { lat, lng },
            words,
            country = "GB",
            nearestPlace = "Test Place",
            language = "en",
            map = $"https://w3w.co/{words}"
        });

        _responses.Add((req =>
            req.RequestUri!.AbsolutePath.Contains("convert-to-coordinates") &&
            req.RequestUri.Query.Contains($"words={Uri.EscapeDataString(words)}"),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            }));
        return this;
    }

    public What3WordsApiClientContextBuilder WithErrorResponse(string path, HttpStatusCode statusCode, string errorCode, string errorMessage)
    {
        var json = JsonSerializer.Serialize(new
        {
            error = new { code = errorCode, message = errorMessage }
        });

        _responses.Add((req =>
            req.RequestUri!.AbsolutePath.Contains(path),
            new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            }));
        return this;
    }

    public IWhat3WordsService Build()
    {
        var handler = new FakeHttpMessageHandler(_responses);
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.what3words.com/v3/") };
        var options = Microsoft.Extensions.Options.Options.Create(new What3WordsOptions { ApiKey = _apiKey });
        return new What3WordsApiClient(httpClient, options);
    }
}

class FakeHttpMessageHandler(
    List<(Func<HttpRequestMessage, bool> predicate, HttpResponseMessage response)> responses)
    : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        foreach (var (predicate, response) in responses)
        {
            if (predicate(request))
                return Task.FromResult(response);
        }

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent("No matching response configured")
        });
    }
}

#region ConvertToAddress

[Subject("What3Words")]
class When_converting_gps_coordinates_to_a_what3words_address
{
    static IWhat3WordsService _service;
    static What3WordsAddress _result;

    Establish context = () =>
        _service = new What3WordsApiClientContextBuilder()
            .WithConvertToAddressResponse(51.520847, -0.195521, "index.home.raft")
            .Build();

    Because of = () =>
        _result = _service.ConvertToAddressAsync(new GpsCoordinate(51.520847, -0.195521)).GetAwaiter().GetResult();

    It should_return_word1 = () => _result.Word1.ShouldEqual("index");
    It should_return_word2 = () => _result.Word2.ShouldEqual("home");
    It should_return_word3 = () => _result.Word3.ShouldEqual("raft");
    It should_format_with_triple_slash_prefix = () => _result.ToString().ShouldEqual("///index.home.raft");
}

[Subject("What3Words")]
class When_converting_coordinates_and_the_api_returns_an_error
{
    static IWhat3WordsService _service;
    static Exception _exception;

    Establish context = () =>
        _service = new What3WordsApiClientContextBuilder()
            .WithErrorResponse("convert-to-3wa", HttpStatusCode.BadRequest, "BadCoordinates", "Invalid coordinates")
            .Build();

    Because of = () =>
        _exception = Catch.Exception(() =>
            _service.ConvertToAddressAsync(new GpsCoordinate(51.520847, -0.195521)).GetAwaiter().GetResult());

    It should_throw_an_exception = () => _exception.ShouldNotBeNull();
    It should_include_the_error_message = () => _exception.Message.ShouldContain("Invalid coordinates");
}

#endregion

#region ConvertToCoordinates

[Subject("What3Words")]
class When_converting_a_what3words_address_to_gps_coordinates
{
    static IWhat3WordsService _service;
    static GpsCoordinate _result;

    Establish context = () =>
        _service = new What3WordsApiClientContextBuilder()
            .WithConvertToCoordinatesResponse("index.home.raft", 51.520847, -0.195521)
            .Build();

    Because of = () =>
        _result = _service.ConvertToCoordinatesAsync(
            new What3WordsAddress("index", "home", "raft")).GetAwaiter().GetResult();

    It should_return_the_latitude = () => _result.Latitude.ShouldEqual(51.520847);
    It should_return_the_longitude = () => _result.Longitude.ShouldEqual(-0.195521);
}

[Subject("What3Words")]
class When_converting_an_invalid_what3words_address
{
    static IWhat3WordsService _service;
    static Exception _exception;

    Establish context = () =>
        _service = new What3WordsApiClientContextBuilder()
            .WithErrorResponse("convert-to-coordinates", HttpStatusCode.BadRequest, "BadWords", "words not found")
            .Build();

    Because of = () =>
        _exception = Catch.Exception(() =>
            _service.ConvertToCoordinatesAsync(
                new What3WordsAddress("not", "real", "words")).GetAwaiter().GetResult());

    It should_throw_an_exception = () => _exception.ShouldNotBeNull();
    It should_include_the_error_message = () => _exception.Message.ShouldContain("words not found");
}

[Subject("What3Words")]
class When_the_what3words_api_is_unreachable
{
    static IWhat3WordsService _service;
    static Exception _exception;

    Establish context = () =>
        _service = new What3WordsApiClientContextBuilder()
            .WithErrorResponse("convert-to-3wa", HttpStatusCode.ServiceUnavailable, "ServerError", "Service unavailable")
            .Build();

    Because of = () =>
        _exception = Catch.Exception(() =>
            _service.ConvertToAddressAsync(new GpsCoordinate(51.520847, -0.195521)).GetAwaiter().GetResult());

    It should_throw_an_exception = () => _exception.ShouldNotBeNull();
}

#endregion
