/*
 * Passing the userid is required for authentication and will be used by chat page
 */

using FindFoodFriends.Firebase.Objects;
namespace FindFoodFriends.Pages;

public partial class UserView : ContentView
{
    private readonly FirebaseUser localuser;
    private readonly ScoreUser scoreuser;

	public UserView(FirebaseUser localuser, ScoreUser scoreuser)
	{
		InitializeComponent();
        this.localuser = localuser;
        this.scoreuser = scoreuser;
        UsernameLabel.Text = scoreuser.DatabaseUser!.Name;
        ScoreLabel.Text = scoreuser.TotalMatchesPercentage;
        DetailsLabel.Text = scoreuser.TrueMatchesEntry;
    }

    private async void Chat_Btn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ChatPage(localuser, scoreuser));
    }
}