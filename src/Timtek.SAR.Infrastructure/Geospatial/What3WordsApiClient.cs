using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Timtek.SAR.Application.Geospatial;
using Timtek.SAR.Domain.ValueObjects;

namespace Timtek.SAR.Infrastructure.Geospatial;

public sealed class What3WordsApiClient(HttpClient httpClient, IOptions<What3WordsOptions> options) : IWhat3WordsService
{
    public async Task<What3WordsAddress> ConvertToAddressAsync(GpsCoordinate coordinate)
    {
        var url = $"convert-to-3wa?coordinates={coordinate.Latitude},{coordinate.Longitude}&key={options.Value.ApiKey}";
        var response = await httpClient.GetAsync(url);
        var body = await response.Content.ReadFromJsonAsync<What3WordsApiResponse>();

        if (!response.IsSuccessStatusCode || body?.Error is not null)
        {
            var message = body?.Error?.Message ?? response.ReasonPhrase ?? "Unknown error";
            throw new What3WordsApiException(message);
        }

        return What3WordsAddress.Parse(body!.Words!);
    }

    public async Task<GpsCoordinate> ConvertToCoordinatesAsync(What3WordsAddress address)
    {
        var words = $"{address.Word1}.{address.Word2}.{address.Word3}";
        var url = $"convert-to-coordinates?words={Uri.EscapeDataString(words)}&key={options.Value.ApiKey}";
        var response = await httpClient.GetAsync(url);
        var body = await response.Content.ReadFromJsonAsync<What3WordsApiResponse>();

        if (!response.IsSuccessStatusCode || body?.Error is not null)
        {
            var message = body?.Error?.Message ?? response.ReasonPhrase ?? "Unknown error";
            throw new What3WordsApiException(message);
        }

        return new GpsCoordinate(body!.Coordinates!.Lat, body.Coordinates.Lng);
    }
}

public sealed class What3WordsApiException(string message) : Exception(message);

file sealed class What3WordsApiResponse
{
    [JsonPropertyName("coordinates")]
    public W3WCoordinates? Coordinates { get; set; }

    [JsonPropertyName("words")]
    public string? Words { get; set; }

    [JsonPropertyName("error")]
    public W3WError? Error { get; set; }
}

file sealed class W3WCoordinates
{
    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lng")]
    public double Lng { get; set; }
}

file sealed class W3WError
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
