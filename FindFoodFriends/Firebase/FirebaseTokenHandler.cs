using FindFoodFriends.Firebase.Objects;
using FindFoodFriends.Firebase.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace FindFoodFriends.Firebase
{
    internal class FirebaseTokenHandler
    {
        /// <summary>
        /// https://firebase.google.com/docs/reference/rest/auth#section-refresh-token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public static async Task<FirebaseUserID?> RefreshIdTokenAsync(string refreshToken)
        {
            var requestData = new
            {
                grant_type = "refresh_token",
                refresh_token = refreshToken,
            };

            var response = await FirebaseClient.Instance.GetClient().PostAsJsonAsync($"{FirebaseEndpoints.RefreshIdTokenEndpoint}?key={FirebaseClient.ApiKey}", requestData);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                FirebaseUserID userIDObject = FirebaseJsonHelper.ConvertJsonObjectToUserID(responseBody)!;
                return userIDObject;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Check if token is expired and return bool
        /// </summary>
        /// <param name="idToken"></param>
        /// <returns></returns>
        public static bool IsTokenExpired(string idToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (tokenHandler.ReadToken(idToken) is not JwtSecurityToken jwtToken)
            {
                // Invalid token format
                return true;
            }

            var now = DateTime.UtcNow;
            var expirationTime = jwtToken.ValidTo;

            return now > expirationTime;
        }

        /// <summary>
        /// Get token from secure storage
        /// </summary>
        /// <returns></returns>
        public static string GetUserIDToken()
        {
            return SecureStorage.GetAsync("useridtoken").ToString()!;
        }

        /// <summary>
        /// Save token in secure storage
        /// </summary>
        /// <param name="token"></param>
        public static void SetUserIdToken(string token)
        {
            SecureStorage.SetAsync("useridtoken", token);
        }
    }
}
