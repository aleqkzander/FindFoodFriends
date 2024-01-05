using FindFoodFriends.Firebase.Utility;

namespace FindFoodFriends.Firebase
{
    internal class FirebaseClient
    {
        public static string? ApiKey = FirebaseEnviromentLoader.Instance.GetApiKey();
        private static readonly Lazy<FirebaseClient> instance = new(() => new FirebaseClient());
        private readonly HttpClient client;

        private FirebaseClient()
        {
            client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        public static FirebaseClient Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public HttpClient GetClient()
        {
            return client;
        }
    }
}
