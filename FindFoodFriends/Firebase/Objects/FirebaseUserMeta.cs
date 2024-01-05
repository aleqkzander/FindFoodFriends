using System.Text.Json.Serialization;

namespace FindFoodFriends.Firebase.Objects
{
    public class FirebaseUserMeta
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("refrences")]
        public List<string>? References { get; set; }

        [JsonPropertyName("location")]
        public UserLocation? Location { get; set; }

        public FirebaseUserMeta() { }
    }
}
