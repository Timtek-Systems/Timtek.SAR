using Timtek.SAR.Application.Geospatial;
using Timtek.SAR.Domain.ValueObjects;

namespace Timtek.SAR.Infrastructure.Geospatial;

/// <summary>
/// Returns placeholder W3W addresses when no API key is configured.
/// </summary>
public sealed class StubWhat3WordsService : IWhat3WordsService
{
    public Task<What3WordsAddress> ConvertToAddressAsync(GpsCoordinate coordinate)
    {
        var hash = HashCode.Combine(
            Math.Round(coordinate.Latitude, 5),
            Math.Round(coordinate.Longitude, 5));
        var words = GenerateWords(Math.Abs(hash));
        return Task.FromResult(words);
    }

    public Task<GpsCoordinate> ConvertToCoordinatesAsync(What3WordsAddress address)
    {
        throw new What3WordsApiException(
            "W3W-to-GPS conversion is not available without a What3Words API key. " +
            "Configure What3Words:ApiKey in appsettings.json.");
    }

    static What3WordsAddress GenerateWords(int seed)
    {
        string[] pool = ["alpha", "bravo", "cedar", "delta", "ember", "frost", "grape", "haven"];
        return new What3WordsAddress(
            pool[seed % pool.Length],
            pool[(seed / pool.Length) % pool.Length],
            pool[(seed / (pool.Length * pool.Length)) % pool.Length]);
    }
}
