namespace FindFoodFriends.Firebase.Utility
{
    internal class FirebaseDataFile
    {
        public static bool IsPresent()
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

        public static void Create(string content)
        {
            string dataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "fff_data");

            if (IsPresent() == false)
            {
                File.WriteAllText(dataFile, content);
            }
        }

        public static string Get()
        {
            string dataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "fff_data");

            if (IsPresent())
            {
                string jsonUser = File.ReadAllText(dataFile);
                return jsonUser;
            }
            else
            {
                return string.Empty;
            }
        }

        public static void Delete()
        {
            string dataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "fff_data");

            if (IsPresent())
            {
                File.Delete(dataFile);
            }
        }
    }
}
