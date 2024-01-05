using FindFoodFriends.Firebase.Objects;

namespace FindFoodFriends.Pages;

public partial class UserView : ContentView
{
	public UserView(ScoreUser scoreUser)
	{
		InitializeComponent();

        UsernameLabel.Text = scoreUser.Meta!.Name;
        ScoreLabel.Text = scoreUser.TotalMatchesPercentage;
        DetailsLabel.Text = scoreUser.TrueMatchesEntry;
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {

    }
}