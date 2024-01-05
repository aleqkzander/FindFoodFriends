/*
*         // Example: User's location
*         double userLatitude = 37.7749; // Example latitude
*         double userLongitude = -122.4194; // Example longitude
*         
*         // Example: Target location
*         double targetLatitude = 37.7833; // Example latitude
*         double targetLongitude = -122.4167; // Example longitude
*         
*         // Example: Radius in kilometers
*         double radiusKm = 10.0;
*         
*         // Check if the target location is within the specified radius
*         bool isWithinRadius = LocationUtils.IsWithinRadius(userLatitude, userLongitude, targetLatitude, targetLongitude, radiusKm);
*         
*         // Display result
*         if (isWithinRadius)
*         {
*         // do something
*         }
*         else
*         {
*         // do something
*         }
*/

namespace FindFoodFriends.Firebase.Utility
{
    internal class RadiusCalculator
    {
        private const double EarthRadiusKm = 6371.0;

        public static bool IsWithinRadius(double userLatitude, double userLongitude, double targetLatitude, double targetLongitude, double radiusKm)
        {
            // Calculate the distance between the user and target locations
            double? distance = CalculateDistance(userLatitude, userLongitude, targetLatitude, targetLongitude);

            // Check if the calculated distance is within the specified radius
            return distance <= radiusKm;
        }

        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Calculate differences in latitude and longitude
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            // Haversine formula to calculate distance
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Calculate the distance in kilometers
            return EarthRadiusKm * c;
        }

        private static double ToRadians(double angle)
        {
            return angle * (Math.PI / 180.0);
        }
    }
}
