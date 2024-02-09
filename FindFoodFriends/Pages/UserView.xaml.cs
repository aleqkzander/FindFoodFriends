/*
 * Passing the userid is required for authentication and will be used by chat page
 */

using FindFoodFriends.Firebase.Objects;
namespace FindFoodFriends.Pages;

public partial class UserView : ContentView
{
    private readonly FirebaseUserID firebaseUserID;
    private readonly ScoreUser scoreuser;

	public UserView(FirebaseUserID firebaseUserID, ScoreUser scoreUser)
	{
		InitializeComponent();
        this.firebaseUserID = firebaseUserID;
        this.scoreuser = scoreUser;
        UsernameLabel.Text = scoreUser.DatabaseUser!.Name;
        ScoreLabel.Text = scoreUser.TotalMatchesPercentage;
        DetailsLabel.Text = scoreUser.TrueMatchesEntry;
    }

    private void Chat_Btn_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new ChatPage(firebaseUserID, scoreuser));
    }
}