/*
 * Passing the userid is required for authentication and will be used by chat page
 */

using FindFoodFriends.Firebase.Objects;
namespace FindFoodFriends.Pages;

public partial class UserView : ContentView
{
    private readonly FirebaseUser localuser;
    private readonly ScoreUser scoreuser;
    private readonly List<FirebaseMessage> initialUserMessages;

	public UserView(FirebaseUser localuser, ScoreUser scoreuser, List<FirebaseMessage> messages)
	{
		InitializeComponent();
        this.localuser = localuser;
        this.scoreuser = scoreuser;
        UsernameLabel.Text = scoreuser.DatabaseUser!.Name;
        ScoreLabel.Text = scoreuser.TotalMatchesPercentage;
        DetailsLabel.Text = scoreuser.TrueMatchesEntry;
        initialUserMessages = messages;
    }

    public UserView(FirebaseUser localuser, ScoreUser scoreuser, List<FirebaseMessage> messages, string lastmessage)
    {
        InitializeComponent();
        this.localuser = localuser;
        this.scoreuser = scoreuser;
        UsernameLabel.Text = scoreuser.DatabaseUser!.Name;
        ScoreLabel.IsVisible = false;
        DetailsLabel.Text = lastmessage;
        initialUserMessages = messages;
    }


    private async void Chat_Btn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ChatPage(localuser, scoreuser, initialUserMessages));
    }

    public ScoreUser GetUser() { return scoreuser; }
}