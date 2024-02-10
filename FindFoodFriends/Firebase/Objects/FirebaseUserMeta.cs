using System.Text.Json.Serialization;

namespace FindFoodFriends.Firebase.Objects
{
    public class FirebaseUserMeta
    {
        [JsonPropertyName("userid")]
        public string? UserId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("location")]
        public UserLocation? Location { get; set; }

        [JsonPropertyName("refrences")]
        public List<string>? References { get; set; }

        [JsonPropertyName("chatrooms")]
        public List<FirebaseChatroom>? Chatrooms { get; set; }

        public FirebaseUserMeta() { }
    }
}
