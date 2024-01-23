using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FindFoodFriends.Firebase.Objects
{
    public class FirebaseChatroom
    {
        [JsonPropertyName("chatroomid")]
        public string? ChatroomID { get; set; }

        [JsonPropertyName("participants")]
        public List<string>? Participants { get; set; }

        [JsonPropertyName("messages")]
        public List<FirebaseMessage>? Messages { get; set; }

        public FirebaseChatroom() { }
    }
}
