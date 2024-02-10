/*
* Official firebase documentation for the REST API: https://firebase.google.com/docs/reference/rest/database?hl=de
*/

using FindFoodFriends.Firebase.Objects;
using FindFoodFriends.Firebase.Utility;
using System.Net.Http.Json;

namespace FindFoodFriends.Firebase
{
    internal class FirebaseDatabase
    {
        /// <summary>
        /// Insert a set new UserMeta
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="userMeta"></param>
        /// <returns></returns>
        public static async Task<string> AddUserMetaAsync(FirebaseUserID userid, FirebaseUserMeta userMeta)
        {
            /*
             * use the localid to get write access
             */

            var response = await FirebaseClient.Instance.GetClient()
                .PutAsJsonAsync($"{FirebaseEndpoints.DatabaseEndpoint}/users/{userid.LocalId}.json?auth={userid.IdToken}", userMeta);

            if (response.IsSuccessStatusCode)
            {
                return "success";
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                return errorMessage;
            }
        }

        /// <summary>
        /// Return the user based on email adress
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static async Task<FirebaseUserMeta?>? GetSpecificMetaDataAsync(FirebaseUserID userid)
        {
            var response = await FirebaseClient.Instance.GetClient()
                .GetAsync($"{FirebaseEndpoints.DatabaseEndpoint}/users/{userid.LocalId}.json?auth={userid.IdToken}");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var userMeta = FirebaseJsonHelper.ConvertJsonObjectToFirebaseUserMeta(responseBody);
                return userMeta;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get a list of meta data
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static async Task<List<FirebaseUserMeta>?> GetAllMetaDataAsync(FirebaseUserID userid)
        {
            List<FirebaseUserMeta> firebaseUserMetaList = new();

            var response = await FirebaseClient.Instance.GetClient()
                .GetAsync($"{FirebaseEndpoints.DatabaseEndpoint}/users.json?auth={userid.IdToken}");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var userDict = FirebaseJsonHelper.ConvertJsonObjectToFirebaseUserMetaDictionary(responseBody)!;

                //foreach (var user in userDict)
                //{
                //    FirebaseUserMeta
                //}

                var usersList = userDict?.Values.ToList();
                return usersList;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Delete a user by his userid
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static async Task<string> DeleteUserMetaAsync(FirebaseUserID userid)
        {
            var response = await FirebaseClient.Instance.GetClient()
                .DeleteAsync($"{FirebaseEndpoints.DatabaseEndpoint}/users/{userid.LocalId}.json?auth={userid.IdToken}");

            if (response.IsSuccessStatusCode)
            {
                return "success";
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                return errorMessage;
            }
        }

        /// <summary>
        /// Store a message at the database
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<string> SendMessageToDatabase(FirebaseUserID userid, FirebaseMessage message)
        {
            // Now, you can write the message under the user's ID
            var messageSentResponse = await FirebaseClient.Instance.GetClient()
                .PostAsJsonAsync($"{FirebaseEndpoints.DatabaseEndpoint}/messages/{userid.LocalId}.json?auth={userid.IdToken}", message);

            if (messageSentResponse.IsSuccessStatusCode)
            {
                return $"success";
            }
            else
            {
                string? errorMessage = await messageSentResponse.Content.ReadAsStringAsync();
                return errorMessage;
            }
        }

        public static async Task<List<FirebaseMessage>?> GetMessagesFromDatabase(FirebaseUserID userid)
        {
            var response = await FirebaseClient.Instance.GetClient()
                .GetAsync($"{FirebaseEndpoints.DatabaseEndpoint}/messages/{userid.LocalId}.json?auth={userid.IdToken}");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var messageDict = FirebaseJsonHelper.ConvertJsonObjectToFirebaseMessageDictionry(responseBody);
                var messagesList = messageDict?.Values.ToList();
                return messagesList;
            }
            else
            {
                return null;
            }
        }
    }
}
