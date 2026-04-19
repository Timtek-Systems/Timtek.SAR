namespace Timtek.SAR.Domain.ValueObjects;

public sealed record GpsCoordinate
{
    public double Latitude { get; }
    public double Longitude { get; }

    public GpsCoordinate(double Latitude, double Longitude)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(Latitude, -90);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(Latitude, 90);
        ArgumentOutOfRangeException.ThrowIfLessThan(Longitude, -180);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(Longitude, 180);

        this.Latitude = Latitude;
        this.Longitude = Longitude;
    }
}
