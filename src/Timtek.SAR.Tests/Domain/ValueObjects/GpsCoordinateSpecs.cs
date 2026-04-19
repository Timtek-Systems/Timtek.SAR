using Timtek.SAR.Domain.ValueObjects;

namespace Timtek.SAR.Tests.Domain.ValueObjects;

[Subject("GPS Coordinate")]
class When_creating_a_gps_coordinate_with_valid_values
{
    static GpsCoordinate _result;

    Because of = () => _result = new GpsCoordinate(51.5074, -0.1278);

    It should_store_the_latitude = () => _result.Latitude.ShouldEqual(51.5074);
    It should_store_the_longitude = () => _result.Longitude.ShouldEqual(-0.1278);
}

[Subject("GPS Coordinate")]
class When_creating_a_gps_coordinate_with_latitude_above_90
{
    static Exception _exception;

    Because of = () => _exception = Catch.Exception(() => new GpsCoordinate(90.1, 0));

    It should_throw_an_argument_out_of_range_exception = () =>
        _exception.ShouldBeOfExactType<ArgumentOutOfRangeException>();
}

[Subject("GPS Coordinate")]
class When_creating_a_gps_coordinate_with_latitude_below_minus_90
{
    static Exception _exception;

    Because of = () => _exception = Catch.Exception(() => new GpsCoordinate(-90.1, 0));

    It should_throw_an_argument_out_of_range_exception = () =>
        _exception.ShouldBeOfExactType<ArgumentOutOfRangeException>();
}

[Subject("GPS Coordinate")]
class When_creating_a_gps_coordinate_with_longitude_above_180
{
    static Exception _exception;

    Because of = () => _exception = Catch.Exception(() => new GpsCoordinate(0, 180.1));

    It should_throw_an_argument_out_of_range_exception = () =>
        _exception.ShouldBeOfExactType<ArgumentOutOfRangeException>();
}

[Subject("GPS Coordinate")]
class When_creating_a_gps_coordinate_with_longitude_below_minus_180
{
    static Exception _exception;

    Because of = () => _exception = Catch.Exception(() => new GpsCoordinate(0, -180.1));

    It should_throw_an_argument_out_of_range_exception = () =>
        _exception.ShouldBeOfExactType<ArgumentOutOfRangeException>();
}

[Subject("GPS Coordinate")]
class When_creating_a_gps_coordinate_at_boundary_values
{
    static GpsCoordinate _northPole;
    static GpsCoordinate _southPole;
    static GpsCoordinate _dateLine;
    static GpsCoordinate _antiDateLine;

    Because of = () =>
    {
        _northPole = new GpsCoordinate(90, 0);
        _southPole = new GpsCoordinate(-90, 0);
        _dateLine = new GpsCoordinate(0, 180);
        _antiDateLine = new GpsCoordinate(0, -180);
    };

    It should_accept_latitude_of_90 = () => _northPole.Latitude.ShouldEqual(90);
    It should_accept_latitude_of_minus_90 = () => _southPole.Latitude.ShouldEqual(-90);
    It should_accept_longitude_of_180 = () => _dateLine.Longitude.ShouldEqual(180);
    It should_accept_longitude_of_minus_180 = () => _antiDateLine.Longitude.ShouldEqual(-180);
}

[Subject("GPS Coordinate")]
class When_comparing_two_gps_coordinates_with_the_same_values
{
    static GpsCoordinate _a;
    static GpsCoordinate _b;

    Establish context = () =>
    {
        _a = new GpsCoordinate(51.5074, -0.1278);
        _b = new GpsCoordinate(51.5074, -0.1278);
    };

    It should_be_equal = () => _a.ShouldEqual(_b);
    It should_have_the_same_hash_code = () => _a.GetHashCode().ShouldEqual(_b.GetHashCode());
}

[Subject("GPS Coordinate")]
class When_comparing_two_gps_coordinates_with_different_values
{
    static GpsCoordinate _a;
    static GpsCoordinate _b;

    Establish context = () =>
    {
        _a = new GpsCoordinate(51.5074, -0.1278);
        _b = new GpsCoordinate(48.8566, 2.3522);
    };

    It should_not_be_equal = () => _a.ShouldNotEqual(_b);
}
