using FindFoodFriends.Firebase;
using FindFoodFriends.Firebase.Objects;
using FindFoodFriends.Firebase.Utility;

namespace FindFoodFriends.Pages;

public partial class ApplicationPage : TabbedPage
{
    private readonly FirebaseUser? localUser;
    private List<FirebaseUserMeta>? userList = [];
    private readonly IList<ScoreUser> userscoresList = [];

    public ApplicationPage(FirebaseUser firebaseUser)
	{
		InitializeComponent();
        localUser = firebaseUser;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        SliderRadius.Value = 10;

        try
        {
            userList = await FirebaseDatabase.GetAllMetaDataAsync(localUser!.UserID!);

            if (userList != null)
            {
                if (userList.Count <= 1)
                {
                    await DisplayAlert("Status", "Du bist bisher der einzige Benutzer :)", "Ok");
                    return;
                }
                else
                {
                    FillTheSearchBox();
                }
            }
        }
        catch
        {
            await DisplayAlert("Error", "Oh das ist etwas schief gelaufen...", "Ok");
        }
    }

    private void ShowLocalData()
    {
        string references = string.Empty;

        foreach (string reference in localUser!.Meta!.References!)
        {
            references += reference + "\n";
        }

        DisplayAlert(localUser.Meta.Name, $"{references}\n" +
            $"Position {localUser.Meta.Location!.Latitude}:{localUser.Meta.Location.Longitude}", "Ok");
    }

    private void FillTheSearchBox()
    {
        try
        {
            SearchBox.Clear();

            foreach (FirebaseUserMeta user in userList!)
            {
                #region create data for assigning 
                string trueMatchesEntrys = string.Empty;
                int totalMatchesScore = 0;
                int trueMatchesScore = 0;
                #endregion create data for assigning

                List<string> tempMatchesList = [];

                #region calculate the score
                for (int i = 0; i < localUser!.Meta!.References!.Count; i++)
                {
                    if (localUser.Meta.References[i] == user.References![i])
                    {
                        // for displaying all matches
                        totalMatchesScore++;

                        if (user.References[i].Trim().Contains("true", StringComparison.OrdinalIgnoreCase))
                        {
                            // Your existing code for handling true matches
                            trueMatchesScore++;

                            int index = user.References[i].IndexOf('=');
                            string filter = user.References[i][index..];
                            string result = user.References[i].Replace(filter, "");

                            if (!localUser.Meta.References.Count.Equals(i - 1))
                                tempMatchesList.Add(result);

                            trueMatchesEntrys = string.Join(", ", tempMatchesList);
                        }
                    }
                }
                #endregion calcualte the score

                #region setup values for displaying compareMatches
                string totalMatchesPercentage = string.Empty;
                if (trueMatchesScore == 1) totalMatchesPercentage = $"{trueMatchesScore} Übereinstimmung";
                else totalMatchesPercentage = $"{trueMatchesScore} Übereinstimmungen";
                #endregion setup values for displaying compareMatches

                #region create an instance of ScoreClass and set its values
                ScoreUser scoreclass = new()
                {
                    Meta = user,
                    TotalMatchesPercentage = totalMatchesPercentage,
                    TrueMatchesEntry = trueMatchesEntrys,
                    TotalMatchesScore = totalMatchesScore,
                    TrueMatchesScore = trueMatchesScore
                };
                #endregion create an instance of ScoreClass and set its values

                #region add the ScoreClass-Dataset to userscores-List
                if (scoreclass != null) userscoresList.Add(scoreclass);
                #endregion add the ScoreClass-Dataset to userscores-List

                #region order the list in desecening order
                // filter
                IOrderedEnumerable<ScoreUser> orderedList = from userscore in userscoresList orderby userscore.TotalMatchesScore descending select userscore;
                #endregion order the list in desecening order

                #region calculate radius
                bool isWithinRadius = RadiusCalculator.IsWithinRadius(
                    (double)localUser.Meta.Location!.Latitude!,
                    (double)localUser.Meta.Location.Longitude!,
                    (double)user.Location!.Latitude!,
                    (double)user.Location.Longitude!,
                    (double)SliderRadius.Value!);
                #endregion calculate radius

                #region add users to screen from userscores
                if (scoreclass!.Meta.Name != localUser.Meta.Name && isWithinRadius)
                {
                    // Create a user data viewmodel
                    UserView dataUser = new(scoreclass);

                    // Add to SearchView
                    SearchBox.Children.Add(dataUser);
                }
                #endregion add users to screen from userscores
            }
        }
        catch
        {
            DisplayAlert("Error", "Oh das ist etwas schief gelaufen...", "Ok");
        }
    }

    private static string CalculateMatchPercentage(int totalMatches)
    {
        double percentage = (double)totalMatches / 24 * 100;
        return $"{percentage:00}%";
    }

    private void ResetLocalDataBtn_Clicked(object sender, EventArgs e)
    {
        FirebaseDataFile.Delete();
        Environment.Exit(0);
    }

    private void SliderRadius_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        SliderLabel.Text = "Suchradius " + SliderRadius.Value.ToString("00") + " km";
    }

    private void SliderRadius_DragCompleted(object sender, EventArgs e)
    {
        FillTheSearchBox();
    }
}