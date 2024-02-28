namespace FindFoodFriends.Firebase.Utility
{
    internal class FirebaseDataFile
    {
        public static bool IsPresentDataFile()
        {
            string dataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "fff_data");

            if (File.Exists(dataFile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsPresentMessageFile()
        {
            string messageFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "fff_message");

            if (File.Exists(messageFile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void CreateData(string content)
        {
            string dataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "fff_data");

            if (!IsPresentDataFile())
            {
                File.WriteAllText(dataFile, content);
            }
        }

        public static string GetData()
        {
            string dataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "fff_data");

            if (IsPresentDataFile())
            {
                string jsonUser = File.ReadAllText(dataFile);
                return jsonUser;
            }
            else
            {
                return string.Empty;
            }
        }

        public static void CreateMessage(string content)
        {
            string messageFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "fff_message");

            if (!IsPresentMessageFile())
            {
                File.WriteAllText(messageFile, content);
            }
        }

        public static string GetMessage()
        {
            string messageFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "fff_message");

            if (IsPresentMessageFile())
            {
                string jsonMessageData = File.ReadAllText(messageFile);
                return jsonMessageData;
            }
            else
            {
                return string.Empty;
            }
        }

        public static void DeleteData()
        {
            string dataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "fff_data");

            if (IsPresentDataFile())
            {
                File.Delete(dataFile);
            }
        }

        public static void DeleteMessage()
        {
            string messageFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "fff_message");

            if (IsPresentDataFile())
            {
                File.Delete(messageFile);
            }
        }
    }
}
