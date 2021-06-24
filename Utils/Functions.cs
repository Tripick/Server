using System;

namespace TripickServer.Utils
{
    public static class Functions
    {
        public static double CoordinatesDistance(double startLat, double startLon, double endLat, double endLon)
        {
            double R = 6371000;
            double startLatRad = startLat * Math.PI / 180;
            double endLatRad = endLat * Math.PI / 180;
            double diffLat = (endLat - startLat) * Math.PI / 180;
            double diffLon = (endLon - startLon) * Math.PI / 180;

            double a = Math.Sin(diffLat / 2) * Math.Sin(diffLat / 2) + Math.Cos(startLatRad) * Math.Cos(endLatRad) * Math.Sin(diffLon / 2) * Math.Sin(diffLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c;
            return d;
        }
    }
}
