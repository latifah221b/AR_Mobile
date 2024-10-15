using System;

public static class GeospatialDistance
{
    private const double EarthRadiusKm = 6371.0; // Radius of the Earth in kilometers

    public static double Haversine(double Latitude1, double Longitude1, double Latitude2, double Longitude2)
    {
        // Convert latitude and longitude from degrees to radians
        double lat1Rad = DegreesToRadians(Latitude1);
        double lon1Rad = DegreesToRadians(Longitude1);
        double lat2Rad = DegreesToRadians(Latitude2);
        double lon2Rad = DegreesToRadians(Longitude2);

        // Haversine formula
        double dlat = lat2Rad - lat1Rad;
        double dlon = lon2Rad - lon1Rad;

        double a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                   Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                   Math.Sin(dlon / 2) * Math.Sin(dlon / 2);
        double c = 2 * Math.Asin(Math.Sqrt(a));

        return EarthRadiusKm * c; // Distance in kilometers
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
}