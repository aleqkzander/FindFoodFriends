/*
* Official firebase documentation for the REST API: https://firebase.google.com/docs/reference/rest/database?hl=de
*/

using FindFoodFriends.Firebase.Objects;
using FindFoodFriends.Firebase.Utility;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;

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
            List<FirebaseUserMeta> firebaseUserMetaList = [];

            var response = await FirebaseClient.Instance.GetClient()
                .GetAsync($"{FirebaseEndpoints.DatabaseEndpoint}/users.json?auth={userid.IdToken}");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var userDict = FirebaseJsonHelper.ConvertJsonObjectToFirebaseUserMetaDictionary(responseBody)!;

                foreach (var user in userDict)
                {
                    FirebaseUserMeta userObject = new();
                    userObject = user.Value;
                    userObject.UserId = user.Key;
                    firebaseUserMetaList.Add(userObject);
                }

                return firebaseUserMetaList;
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
        public static async Task<string> SendMessageToDatabase(FirebaseUserID userid, string foreignUserId, FirebaseMessage message)
        {
            // this is your user
            var useridMessageSentResponse = await FirebaseClient.Instance.GetClient()
                .PostAsJsonAsync($"{FirebaseEndpoints.DatabaseEndpoint}/messages/{userid.LocalId}.json?auth={userid.IdToken}", message);

            // this is the other user
            var foreignUserIdMessageSentResponse = await FirebaseClient.Instance.GetClient()
                .PostAsJsonAsync($"{FirebaseEndpoints.DatabaseEndpoint}/messages/{foreignUserId}.json?auth={userid.IdToken}", message);

            if (useridMessageSentResponse.IsSuccessStatusCode && foreignUserIdMessageSentResponse.IsSuccessStatusCode)
            {
                return $"success";
            }
            else
            {
                string? errorMessage = await useridMessageSentResponse.Content.ReadAsStringAsync();
                return errorMessage;
            }
        }

        //public static async Task<List<FirebaseMessage>?> GetMessagesFromDatabase(FirebaseUserID userid)
        //{
        //    List<FirebaseMessage> firebaseMessages = [];

        //    var response = await FirebaseClient.Instance.GetClient()
        //        .GetAsync($"{FirebaseEndpoints.DatabaseEndpoint}/messages/{userid.LocalId}.json?auth={userid.IdToken}");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string responseBody = await response.Content.ReadAsStringAsync();
        //        var messageDict = FirebaseJsonHelper.ConvertJsonObjectToFirebaseMessageDictionry(responseBody)!;

        //        foreach (var message in messageDict)
        //        {
        //            FirebaseMessage firebaseMessage = new();
        //            firebaseMessage = message.Value;
        //            firebaseMessage.MessageId = message.Key;
        //            firebaseMessages.Add(firebaseMessage);
        //        }

        //        return firebaseMessages;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// Use method to download all messages from a user in a seperate client instance
        /// </summary>
        /// <param name="client"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static async Task<List<FirebaseMessage>?> DownloadAllMessages(HttpClient client, FirebaseUserID userid)
        {
            List<FirebaseMessage> firebaseMessages = [];

            var response = await client
                .GetAsync($"{FirebaseEndpoints.DatabaseEndpoint}/messages/{userid.LocalId}.json?auth={userid.IdToken}");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var messageDict = FirebaseJsonHelper.ConvertJsonObjectToFirebaseMessageDictionry(responseBody)!;

                foreach (var message in messageDict)
                {
                    FirebaseMessage firebaseMessage = new();
                    firebaseMessage = message.Value;
                    firebaseMessage.MessageId = message.Key;
                    firebaseMessages.Add(firebaseMessage);
                }

                return firebaseMessages;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get a count of messages
        /// </summary>
        /// <param name="client"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static async Task<int> CountMessagesForUser(HttpClient client, FirebaseUserID userid)
        {
            try
            {
                var response = await client
                    .GetAsync($"{FirebaseEndpoints.DatabaseEndpoint}/messages/{userid.LocalId}.json?auth={userid.IdToken}&shallow=true");

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var messageKeys = JObject.Parse(responseBody);
                    return messageKeys.Count;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Get a specific amount of messages
        /// </summary>
        /// <param name="client"></param>
        /// <param name="userid"></param>
        /// <param name="messagecount"></param>
        /// <returns></returns>
        public static async Task<List<FirebaseMessage>?> DownloadAmountOfMessages(HttpClient client, FirebaseUserID userid, int messagecount)
        {
            List<FirebaseMessage> firebaseMessages = [];

            var response = await client
                .GetAsync($"{FirebaseEndpoints.DatabaseEndpoint}/messages/{userid.LocalId}.json?auth={userid.IdToken}&orderBy=\"$key\"&limitToLast={messagecount}");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var messageDict = FirebaseJsonHelper.ConvertJsonObjectToFirebaseMessageDictionry(responseBody)!;

                foreach (var message in messageDict)
                {
                    FirebaseMessage firebaseMessage = message.Value;
                    firebaseMessage.MessageId = message.Key;
                    firebaseMessages.Add(firebaseMessage);
                }

                return firebaseMessages;
            }
            else
            {
                return null;
            }
        }
    }
}
