/*
 * On this page all user data will be deleted from the device
 * Here we make sure the user is registered
 */

using FindFoodFriends.Firebase;
using FindFoodFriends.Firebase.Objects;
using FindFoodFriends.Firebase.Utility;

namespace FindFoodFriends.Pages;

public partial class LoginPage : ContentPage
{
    string? userMail;
    string? userPassword;

    public LoginPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        FirebaseDataFile.Delete();
    }

    private async void LoginBtn_Clicked(object sender, EventArgs e)
    {
        if (RequiredDataProvided())
            await ExecuteRequest();
    }

    private bool RequiredDataProvided()
    {
        userMail = MailEntry.Text;
        userPassword = PasswordEntry.Text;

        if (string.IsNullOrEmpty(userMail) || string.IsNullOrEmpty(userPassword))
        {
            DisplayAlert("Fehler", "Bitte gib die benötigten Daten an", "Ok");
            return false;
        }
        else
        {
            return true;
        }
    }

    private async Task ExecuteRequest()
    {
        try
        {
            SetStatusExecutingRequest();
            FirebaseUserID? userIdData = await FirebaseAuthentication.LoginUserAsync(userMail!, userPassword!);

            if (userIdData == null)
            {
                await DisplayAlert("Fehler", "Oops das waren die falschen Daten...", "Ok");
                SetStatusLogin();
                return;
            }
            else
            {
                FirebaseUser firebaseUser = new()
                {
                    UserID = userIdData
                };

                FirebaseUserMeta? firebaseUserMeta = await FirebaseDatabase.GetSpecificMetaDataAsync(userIdData)!;

                if (firebaseUserMeta == null)
                {
                    await Navigation.PushModalAsync(new MetaInformationPage(firebaseUser));
                }
                else
                {
                    firebaseUser.Meta = firebaseUserMeta;

                    // create a new local file
                    string firebaseUserJson = FirebaseJsonHelper.ConvertFirebaseUserToJsonObject(firebaseUser)!;
                    FirebaseDataFile.Create(firebaseUserJson);

                    await Navigation.PushModalAsync(new ApplicationPage(firebaseUser));
                }
            }
        }
        catch
        {
            await DisplayAlert("Error", "Oh das ist etwas schief gelaufen...", "Ok");
            SetStatusLogin();
        }
    }

    private void SetStatusLogin()
    {
        LoginBtn.IsEnabled = true;
        MailEntry.IsEnabled = true;
        PasswordEntry.IsEnabled = true;
        LoginBtn.Text = "Bestätigen";
    }

    private void SetStatusExecutingRequest()
    {
        LoginBtn.IsEnabled = false;
        MailEntry.IsEnabled = false;
        PasswordEntry.IsEnabled = false;
        LoginBtn.Text = "Bitte warten";
    }
}