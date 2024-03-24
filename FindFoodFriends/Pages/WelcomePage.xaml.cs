using FindFoodFriends.Firebase.Utility;

namespace FindFoodFriends.Pages;

public partial class WelcomePage : ContentPage
{
    private string? infoText;

    public WelcomePage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        infoText = await DownloadInfoTextAsync();
    }

    private void InfoBtn_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Über find-food-friends", $"{infoText}", "Ok");
    }

    private void RegisterBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new AuthenticationPage());
    }

    private void LoginBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new LoginPage());
    }

    private async Task<string> DownloadInfoTextAsync()
    {
        InfoBtn.IsEnabled = false;

        try
        {
            using var client = new HttpClient();
            string text = await client.GetStringAsync(FirebaseEnviromentLoader.Instance.GetInfoTextEndpoint());
            InfoBtn.Text = "Über die App";
            InfoBtn.IsEnabled = true;
            
            return text;
        }
        catch
        {
            InfoBtn.Text = "Ladefehler...";
            return "Es gab einen Fehler bei der Anfrage...";
        }
    }
}