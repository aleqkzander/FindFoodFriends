using FindFoodFriends.Firebase.Objects;

namespace FindFoodFriends.Pages;

public partial class UserView : ContentView
{
    private readonly ScoreUser scoreuser;

	public UserView(ScoreUser scoreUser)
	{
		InitializeComponent();
        this.scoreuser = scoreUser;
        UsernameLabel.Text = scoreUser.Meta!.Name;
        ScoreLabel.Text = scoreUser.TotalMatchesPercentage;
        DetailsLabel.Text = scoreUser.TrueMatchesEntry;
    }

    private void Chat_Btn_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new ChatPage(scoreuser));
    }
}