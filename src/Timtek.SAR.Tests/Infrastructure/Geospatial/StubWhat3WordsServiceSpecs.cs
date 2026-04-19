using Timtek.SAR.Application.Geospatial;
using Timtek.SAR.Domain.ValueObjects;
using Timtek.SAR.Infrastructure.Geospatial;

namespace Timtek.SAR.Tests.Infrastructure.Geospatial;

[Subject("What3Words")]
class When_using_the_stub_service_to_convert_coordinates_to_an_address
{
    static IWhat3WordsService _service;
    static What3WordsAddress _result;

    Establish context = () =>
        _service = new StubWhat3WordsService();

    Because of = () =>
        _result = _service.ConvertToAddressAsync(new GpsCoordinate(51.520847, -0.195521)).GetAwaiter().GetResult();

    It should_return_a_valid_three_word_address = () =>
    {
        _result.Word1.ShouldNotBeEmpty();
        _result.Word2.ShouldNotBeEmpty();
        _result.Word3.ShouldNotBeEmpty();
    };

    It should_format_with_triple_slash_prefix = () =>
        _result.ToString().ShouldStartWith("///");
}

[Subject("What3Words")]
class When_using_the_stub_service_to_convert_the_same_coordinates_twice
{
    static IWhat3WordsService _service;
    static What3WordsAddress _first;
    static What3WordsAddress _second;

    Establish context = () =>
        _service = new StubWhat3WordsService();

    Because of = () =>
    {
        _first = _service.ConvertToAddressAsync(new GpsCoordinate(51.520847, -0.195521)).GetAwaiter().GetResult();
        _second = _service.ConvertToAddressAsync(new GpsCoordinate(51.520847, -0.195521)).GetAwaiter().GetResult();
    };

    It should_return_the_same_address = () => _first.ShouldEqual(_second);
}

[Subject("What3Words")]
class When_using_the_stub_service_to_convert_w3w_to_coordinates
{
    static IWhat3WordsService _service;
    static Exception _exception;

    Establish context = () =>
        _service = new StubWhat3WordsService();

    Because of = () =>
        _exception = Catch.Exception(() =>
            _service.ConvertToCoordinatesAsync(new What3WordsAddress("index", "home", "raft")).GetAwaiter().GetResult());

    It should_throw_explaining_api_key_is_required = () =>
        _exception.ShouldBeOfExactType<What3WordsApiException>();

    It should_include_configuration_guidance = () =>
        _exception.Message.ShouldContain("ApiKey");
}
