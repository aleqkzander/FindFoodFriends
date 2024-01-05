using System.Text.Json.Serialization;

namespace FindFoodFriends.Firebase.Objects
{
    public class FirebaseUserID
    {
        [JsonPropertyName("idToken")]
        public string? IdToken { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("refreshToken")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("expiresIn")]
        public string? ExpiresIn { get; set; }

        [JsonPropertyName("localId")]
        public string? LocalId { get; set; }

        [JsonPropertyName("registered")]
        public bool Regsitered { get; set; }

        public FirebaseUserID() { }
    }
}
