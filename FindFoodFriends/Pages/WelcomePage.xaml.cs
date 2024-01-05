namespace FindFoodFriends.Pages;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
	}

    private void RegisterBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new AuthenticationPage());
    }

    private void LoginBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new LoginPage());
    }
}