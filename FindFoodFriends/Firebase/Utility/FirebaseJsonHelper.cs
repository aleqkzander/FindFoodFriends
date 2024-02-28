using FindFoodFriends.Firebase.Objects;
using System.Text.Json;

namespace FindFoodFriends.Firebase.Utility
{
    internal class FirebaseJsonHelper
    {
        public static string? ConvertUserIDToJsonObject(FirebaseUserID userID)
        {
            return JsonSerializer.Serialize(userID);
        }

        public static FirebaseUserID? ConvertJsonObjectToUserID(string jsonObject)
        {
            return JsonSerializer.Deserialize<FirebaseUserID>(jsonObject);
        }

        public static string? ConvertFirebaseUserToJsonObject(FirebaseUser user)
        {
            return JsonSerializer.Serialize(user);
        }

        public static FirebaseUser? ConvertJsonObjectToFirebaseUser(string jsonObject)
        {
            return JsonSerializer.Deserialize<FirebaseUser>(jsonObject);
        }

        public static string? ConvertFirebaseUserMetaToJsonObject(FirebaseUserMeta userMeta)
        {
            return JsonSerializer.Serialize(userMeta);
        }

        public static FirebaseUserMeta? ConvertJsonObjectToFirebaseUserMeta(string jsonObject)
        {
            return JsonSerializer.Deserialize<FirebaseUserMeta>(jsonObject);
        }

        public static string? ConvertUserLocationToJsonObject(UserLocation location)
        {
            return JsonSerializer.Serialize(location);
        }

        public static UserLocation? ConvertJsonObjectToUserLocation(string jsonObject)
        {
            return JsonSerializer.Deserialize<UserLocation>(jsonObject);
        }

        public static string? ConvertFirebaseUserMetaListToJsonObject(List<FirebaseUserMeta> userMetaList)
        {
            return JsonSerializer.Serialize(userMetaList);
        }

        public static Dictionary<string, FirebaseUserMeta>? ConvertJsonObjectToFirebaseUserMetaDictionary(string jsonObject)
        {
            return JsonSerializer.Deserialize<Dictionary<string, FirebaseUserMeta>>(jsonObject);
        }

        public static Dictionary<string, FirebaseMessage>? ConvertJsonObjectToFirebaseMessageDictionry(string jsonObject)
        {
            return JsonSerializer.Deserialize<Dictionary<string, FirebaseMessage>>(jsonObject);
        }


        public static List<FirebaseMessage>? ConvertJsonObjectToMessagesList(string json)
        {
            return JsonSerializer.Deserialize<List<FirebaseMessage>>(json);
        }

        public static string ConvertMessagesListToJsonObject(List<FirebaseMessage> messagesList)
        {
            return JsonSerializer.Serialize(messagesList);
        }
    }
}
 