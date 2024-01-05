using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace FindFoodFriends.Firebase.Utility
{
    internal class FirebaseEnviromentLoader
    {
        private static volatile FirebaseEnviromentLoader? instance;
        private static readonly object syncRoot = new();
        private readonly IConfiguration _configuration;

        private FirebaseEnviromentLoader()
        {
            var getAssembly = Assembly.GetExecutingAssembly();
            using var stream = getAssembly.GetManifestResourceStream("FindFoodFriends.appsettings.json");

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream!)
                .Build();

            _configuration = config;
        }

        public static FirebaseEnviromentLoader Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        instance ??= new FirebaseEnviromentLoader();
                    }
                }
                return instance;
            }
        }

        public string GetApiKey() { return _configuration["FIREBASE_APIKEY"]!; }
        public string GetSingUpEndpoint() { return _configuration["FIREBASE_SIGNUP_ENDPOINT"]!; }
        public string GetSingInEndpoint() { return _configuration["FIREBASE_SIGNIN_ENDPOINT"]!; }
        public string GetUserDataEndpoint() { return _configuration["FIREBASE_GETUSERDATA_ENDPOINT"]!; }
        public string RefreshIdTokenEndpoint() { return _configuration["FIREBASE_REFRESHIDTOKEN_ENDPOINT"]!; }
        public string GetDatabaseEndpoint() { return _configuration["FIREBASE_DATABASE_ENDPOINT"]!; }
    }
}
