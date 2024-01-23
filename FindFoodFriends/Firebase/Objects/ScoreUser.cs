namespace FindFoodFriends.Firebase.Objects
{
    public class ScoreUser
    {
        public FirebaseUserMeta? LocalUser { get; set; }
        public FirebaseUserMeta? DatabaseUser { get; set; }
        public string? Score { get; set; }
        public string? TotalMatchesPercentage { get; set; }
        public string? TrueMatchesEntry { get; set; }
        public int? TotalMatchesScore { get; set; }
        public int? TrueMatchesScore { get; set; }

        public ScoreUser() { }
    }
}
