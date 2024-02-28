/*
 * Passing the userid is required for authentication and will be used by chat page
 */

using FindFoodFriends.Firebase.Objects;
namespace FindFoodFriends.Pages;

public partial class ContactCard : ContentView
{
    private readonly FirebaseUser localuser;
    private readonly ScoreUser scoreuser;
    private readonly List<FirebaseMessage> initialUserMessages;

	public ContactCard(FirebaseUser localuser, ScoreUser scoreuser, List<FirebaseMessage> messages)
	{
		InitializeComponent();
        this.localuser = localuser;
        this.scoreuser = scoreuser;
        Chat_Btn.Text = $"Chatten mit {scoreuser.DatabaseUser!.Name}";
        ScoreLabel.Text = scoreuser.TotalMatchesPercentage;
        DetailsLabel.Text = scoreuser.TrueMatchesEntry;
        initialUserMessages = messages;
    }

    public ContactCard(FirebaseUser localuser, ScoreUser scoreuser, List<FirebaseMessage> messages, string name, string lastmessage)
    {
        InitializeComponent();
        this.localuser = localuser;
        this.scoreuser = scoreuser;
        Chat_Btn.Text = $"{scoreuser.DatabaseUser!.Name}";
        ScoreLabel.Text = name;
        DetailsLabel.Text = lastmessage;
        initialUserMessages = messages;
    }


    private async void Chat_Btn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ChatPage(localuser, scoreuser, initialUserMessages));
    }

    public ScoreUser GetUser() { return scoreuser; }
}