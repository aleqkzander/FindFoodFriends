using FindFoodFriends.Firebase.Objects;
using FindFoodFriends.Firebase.Utility;
using System.Net.Http.Json;

namespace FindFoodFriends.Firebase
{
    internal class FirebaseAuthentication
    {
        /// <summary>
        /// https://firebase.google.com/docs/reference/rest/auth#section-create-email-password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<FirebaseUserID?>? RegisterUserAsync(string email, string password)
        {
            /*
             * var registerResult = SignUpAsync("user@example.com", "password").Result;
             */

            var requestData = new
            {
                email,
                password,
                returnSecureToken = true
            };

            var response = await FirebaseClient.Instance.GetClient().PostAsJsonAsync($"{FirebaseEndpoints.SignUpEndpoint}?key={FirebaseClient.ApiKey}", requestData);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                FirebaseUserID userIDObject = FirebaseJsonHelper.ConvertJsonObjectToUserID(responseBody)!;
                userIDObject.Regsitered = true; // The response body don't contain the registered information so set the registered flag manually
                return userIDObject;
            }
            else return null;
        }

        /// <summary>
        /// https://firebase.google.com/docs/reference/rest/auth#section-sign-in-email-password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<FirebaseUserID?> LoginUserAsync(string email, string password)
        {
            /*
             * var loginResult = SignInAsync("user@example.com", "password").Result;
             */

            var requestData = new
            {
                email,
                password,
                returnSecureToken = true
            };

            var response = await FirebaseClient.Instance.GetClient().PostAsJsonAsync($"{FirebaseEndpoints.SignInEndpoint}?key={FirebaseClient.ApiKey}", requestData);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                FirebaseUserID? userIDObject = FirebaseJsonHelper.ConvertJsonObjectToUserID(responseBody); // The repsonse body contain the registered flag on login

                // refresh when expired
                if (FirebaseTokenHandler.IsTokenExpired(userIDObject!.IdToken!))
                {
                    userIDObject = new();
                    userIDObject = await FirebaseTokenHandler.RefreshIdTokenAsync(userIDObject.RefreshToken!);
                }

                return userIDObject;
            }
            else return null;
        }

        /// <summary>
        /// https://firebase.google.com/docs/reference/rest/auth#section-get-account-info
        /// </summary>
        /// <param name="idToken"></param>
        /// <returns></returns>
        public static async Task<FirebaseUserID?> GetUserIdDataAsync(string idToken)
        {
            /*
             *  var idToken = "USER_ID_TOKEN_FROM_SIGN_IN_RESULT";
             *  var userdata = GetUserDataAsync(idToken).Result;
             */

            var requestData = new
            {
                idToken
            };

            var response = await FirebaseClient.Instance.GetClient().PostAsJsonAsync($"{FirebaseEndpoints.GetUserDataEndpoint}?key={FirebaseClient.ApiKey}", requestData);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                FirebaseUserID? userIDObject = FirebaseJsonHelper.ConvertJsonObjectToUserID(responseBody);
                return userIDObject;
            }
            else return null;
        }
    }
}
