using System.Text.Json.Serialization;

namespace FindFoodFriends.Firebase.Objects
{
    public class FirebaseMessage(string sender, string receiver, string message, DateTime timestamp)
    {
        [JsonPropertyName("sender")]
        public string Sender = sender;

        [JsonPropertyName("receiver")]
        public string Receiver = receiver;

        [JsonPropertyName("message")]
        public string Message = message;

        [JsonPropertyName("timestamp")]
        public string Timestamp = timestamp.ToString($"dd.MM.yyyy - HH:mm:ss");
    }
}
