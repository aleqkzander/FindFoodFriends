using System.Text.Json.Serialization;

namespace FindFoodFriends.Firebase.Objects
{
    public class FirebaseMessage
    {
        [JsonPropertyName("messageid")]
        public string? MessageId { get; set; }

        [JsonPropertyName("sender")]
        public string? Sender { get; set; }

        [JsonPropertyName("receiver")]
        public string? Receiver { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; set; }

        public FirebaseMessage() { }

        public FirebaseMessage(string sender, string receiver, string message, DateTime timestamp)
        {
            Sender = sender;
            Receiver = receiver;
            Message = message;
            Timestamp = timestamp.ToString($"dd.MM.yyyy - HH:mm:ss");
        }
    }
}
