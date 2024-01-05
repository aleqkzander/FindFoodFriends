using FindFoodFriends.Firebase.Utility;

namespace FindFoodFriends.Firebase
{
    internal class FirebaseEndpoints
    {
        public static string? SignUpEndpoint = FirebaseEnviromentLoader.Instance.GetSingUpEndpoint();
        public static string? SignInEndpoint = FirebaseEnviromentLoader.Instance.GetSingInEndpoint();
        public static string? GetUserDataEndpoint = FirebaseEnviromentLoader.Instance.GetUserDataEndpoint();
        public static string? RefreshIdTokenEndpoint = FirebaseEnviromentLoader.Instance.RefreshIdTokenEndpoint();
        public static string? DatabaseEndpoint = FirebaseEnviromentLoader.Instance.GetDatabaseEndpoint();
    }
}
