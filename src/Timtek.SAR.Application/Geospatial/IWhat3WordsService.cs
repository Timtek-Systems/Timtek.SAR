using Timtek.SAR.Domain.ValueObjects;

namespace Timtek.SAR.Application.Geospatial;

public interface IWhat3WordsService
{
    Task<What3WordsAddress> ConvertToAddressAsync(GpsCoordinate coordinate);
    Task<GpsCoordinate> ConvertToCoordinatesAsync(What3WordsAddress address);
}
