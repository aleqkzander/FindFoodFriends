using FindFoodFriends.Firebase;
using FindFoodFriends.Firebase.Objects;
using FindFoodFriends.Pages;

namespace FindFoodFriends.Pages;

public partial class AuthenticationPage : ContentPage
{
    readonly FirebaseUser firebaseUser = new();

    public AuthenticationPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        AuthenticateBtn.IsEnabled = false;
    }

    private void DatenschutzerklärungButton_Clicked(object sender, EventArgs e)
    {
        Uri link = new("https://learn.microsoft.com/en-us/xamarin/");
        try
        {
            Browser.OpenAsync(link, BrowserLaunchMode.SystemPreferred);
        }
        catch
        {
            DisplayAlert("Error", "Oh das ist etwas schief gelaufen...", "Ok");
        }
    }

    private async void DatenschutzCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(MailEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text) || string.IsNullOrEmpty(PasswordRepeatEntry.Text))
        {
            await DisplayAlert("Info", "E-Mail oder Passwort nicht ausgefüllt.", "Ok");
            DatenschutzCheckBox.IsChecked = false;
            AuthenticateBtn.IsEnabled = false;
            return;
        }
        else if (PasswordRepeatEntry.Text != PasswordEntry.Text)
        {
            await DisplayAlert("Info", "Die Passwörter stimmen nicht überein.", "Ok");
            DatenschutzCheckBox.IsChecked = false;
            AuthenticateBtn.IsEnabled = false;
            return;
        }
        else
        {
            AuthenticateBtn.IsEnabled = true;
        }
    }

    private async void AuthenticateBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await AuthenticateUser(MailEntry.Text.ToLower(), PasswordEntry.Text);
        }
        catch
        {
            await DisplayAlert("Error", "Oh das ist etwas schief gelaufen...", "Ok");
            AuthenticateBtn.IsEnabled = true;
        }
    }

    private async Task AuthenticateUser(string mail, string password)
    {
        try
        {
            SetStatusAuthenticating();

            FirebaseUserID? firebaseUserId = await FirebaseAuthentication.RegisterUserAsync(mail!, password)!;

            if (firebaseUserId == null)
            {
                await DisplayAlert("Info", "Diese Benutzerdaten sind nicht gültig...", "Ok");
                SetStatusIdle();
                return;
            }
            else
            {
                firebaseUser.UserID = firebaseUserId;
                await Navigation.PushModalAsync(new MetaInformationPage(firebaseUser));
            }
        }
        catch
        {
            await DisplayAlert("Error", "Oh das ist etwas schief gelaufen...", "Ok");
            SetStatusIdle();
        }
    }

    private void SetStatusAuthenticating()
    {
        AuthenticateBtn.IsEnabled = false;
        AuthenticateBtn.Text = "Authentifiziere...";
    }

    private void SetStatusIdle()
    {
        AuthenticateBtn.IsEnabled = true;
        AuthenticateBtn.Text = "Registrieren";
    }
}