using System.Text.Json.Serialization;

namespace FindFoodFriends.Firebase.Objects
{
    public class Reference
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("state")]
        public bool State { get; set; }

        public Reference(string Name, bool State) 
        {
            this.Name = Name;
            this.State = State;
        }
    }
}
