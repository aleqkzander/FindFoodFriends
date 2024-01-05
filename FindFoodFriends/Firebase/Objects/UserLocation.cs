using System.Text.Json.Serialization;

namespace FindFoodFriends.Firebase.Objects
{
    public class UserLocation
    {
        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }

        public UserLocation() { }
    }
}
