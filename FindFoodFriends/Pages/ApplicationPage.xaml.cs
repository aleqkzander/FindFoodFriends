using FindFoodFriends.Firebase;
using FindFoodFriends.Firebase.Objects;
using FindFoodFriends.Firebase.Utility;
using System.Runtime.InteropServices;

namespace FindFoodFriends.Pages;

public partial class ApplicationPage : TabbedPage
{
    private readonly FirebaseUser? localUser;
    private readonly List<FirebaseMessage> initialUserMessages = [];
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
        await Dispatcher.DispatchAsync(EnableLoadingAnimation);

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
                    await DownloadAllMessagesFromUser(localUser);
                    FillTheSearchBox();
                    FillMessagesBox();
                }
            }
        }
        catch
        {
            await DisplayAlert("Error", "Oh das ist etwas schief gelaufen...", "Ok");
        }
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
                    //LocalUser = localUser.Meta,
                    DatabaseUser = user,
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
                if (scoreclass!.DatabaseUser.Name != localUser.Meta.Name && isWithinRadius)
                {
                    // Create a user data viewmodel
                    ContactCard dataUser = new(localUser, scoreclass, initialUserMessages);

                    // Add to SearchView
                    SearchBox.Children.Add(dataUser);
                }
                #endregion add users to screen from userscores
            }

            DisableLoadingAnimation();
        }
        catch
        {
            DisplayAlert("Error", "Oh das ist etwas schief gelaufen...", "Ok");
        }
    }

    private void FillMessagesBox()
    {
        if (initialUserMessages.Count == 0) return;
        MessagesBox.Clear();

        Dictionary<string, FirebaseMessage> lastMessages = [];
        foreach (var message in initialUserMessages)
        {
            if (message.Sender == localUser!.Meta!.Name || message.Receiver == localUser!.Meta!.Name)
            {
                // Update lastMessages for the sender
                if (lastMessages.TryGetValue(message.Sender!, out var existingMessage) && message.Timestamp != existingMessage.Timestamp)
                    lastMessages[message.Sender!] = message;
                else
                    lastMessages[message.Sender!] = message;

                // Update lastMessages only for the receiver
                if (lastMessages.TryGetValue(message.Receiver!, out var existingMessage2) && message.Timestamp != existingMessage2.Timestamp)
                    lastMessages[message.Receiver!] = message;
                else
                    lastMessages[message.Receiver!] = message;
            }
        }

        foreach (var message in lastMessages)
        {
            var lastMessage = message.Value;
            var scoreuser = userscoresList.FirstOrDefault(user => user.DatabaseUser?.Name == message.Key);
            if (scoreuser != null && scoreuser.DatabaseUser!.Name != localUser!.Meta!.Name)
            {
                ContactCard dataUser = new(localUser!, scoreuser, initialUserMessages,  lastMessage.Sender!, lastMessage.Message!);
                MessagesBox.Children.Add(dataUser);
            }
        }
    }

    private void ResetLocalDataBtn_Clicked(object sender, EventArgs e)
    {
        FirebaseDataFile.Delete();
        Environment.Exit(0);
    }

    private async void SliderRadius_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        await Dispatcher.DispatchAsync(EnableLoadingAnimation);
        SliderLabel.Text = "Suchradius " + SliderRadius.Value.ToString("00") + " km";
    }

    private void SliderRadius_DragCompleted(object sender, EventArgs e)
    {
        FillTheSearchBox();
    }

    private async void ChangeUserDataBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            string deletationResult = await FirebaseDatabase.DeleteUserMetaAsync(localUser!.UserID!);

            if (deletationResult == "success")
            {
                FirebaseDataFile.Delete();
                await DisplayAlert("Info", "Deine Daten wurden entfernt. Beim nächsten Login kannst du deine Daten ändern.", "Ok");
                Environment.Exit(0);
            }
            else
            {
                await DisplayAlert("Error", "Oh das ist etwas schief gelaufen. Die Daten wurden nicht gelöscht.", "Ok");
            }
        }
        catch
        {
            await DisplayAlert("Error", "Oh das ist etwas schief gelaufen...", "Ok");
        }
    }

    private async Task EnableLoadingAnimation()
    {
        await Task.Delay(1);
        loading.IsAnimationPlaying = true;
    }

    private void DisableLoadingAnimation()
    {
        loading.IsAnimationPlaying = false;
        loading.IsVisible = false;
    }

    private async Task DownloadAllMessagesFromUser(FirebaseUser firebaseUser)
    {
        try
        {
            using HttpClient client = new();
            List<FirebaseMessage>? messageList = await FirebaseDatabase.DownloadAllMessages(client, firebaseUser!.UserID!);

            if (messageList?.Count != 0)
            {
                foreach (FirebaseMessage message in messageList!)
                {
                    initialUserMessages.Add(message);
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// Call this method pass in an updated messages list
    /// </summary>
    /// <param name="messages"></param>
    public void UpdateInitialUserMessages(List<FirebaseMessage> messages)
    {
        if (messages.Count != 0)
        {
            foreach (var message in messages)
            {
                initialUserMessages.Add(message);
            }
        }
    }
}