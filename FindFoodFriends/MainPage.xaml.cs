using FindFoodFriends.Firebase;
using FindFoodFriends.Firebase.Objects;
using FindFoodFriends.Firebase.Utility;
using FindFoodFriends.Pages;

namespace FindFoodFriends
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(1);
            EnableLoadingAnimation();
            LoadLocalData();
        }

        private void LoadLocalData()
        {
            if (FirebaseDataFile.IsPresent() == false)
            {
                OpenWelcome();
                return;
            }
            else
            {
                LoadFirebaseDataFileAndCreateUser();
            }
        }

        private async void LoadFirebaseDataFileAndCreateUser()
        {
            try
            {
                FirebaseUser? localFirebaseUser = FirebaseJsonHelper.ConvertJsonObjectToFirebaseUser(FirebaseDataFile.Get());
                await CheckUserTokenAsync(localFirebaseUser!);
            }
            catch
            {
                await DisplayAlert("Error", "Oh das ist etwas schief gelaufen...", "Ok");
            }
        }

        private async Task CheckUserTokenAsync(FirebaseUser firebaseUser)
        {
            try
            {
                if (FirebaseTokenHandler.IsTokenExpired(firebaseUser.UserID!.IdToken!))
                {
                    await DisplayAlert("Info", "Aus Sicherheitsgründen habe ich dich ausgeloggt. Melde dich bitte ernaut an.", "Ok");
                    await Navigation.PushModalAsync(new LoginPage());
                }
                else
                {
                    FirebaseUser? localFirebaseUser = FirebaseJsonHelper.ConvertJsonObjectToFirebaseUser(FirebaseDataFile.Get());
                    if (firebaseUser.Meta != null) OpenMainApplication(firebaseUser);
                    else OpenMetaInformation(firebaseUser);
                }
            }
            catch
            {
                await DisplayAlert("Error", "Oh das ist etwas schief gelaufen...", "Ok");
            }
        }

        private void EnableLoadingAnimation()
        {
            loading.IsAnimationPlaying = true;
        }

        private void OpenWelcome()
        {
            Navigation.PushModalAsync(new WelcomePage());
        }

        private void OpenMainApplication(FirebaseUser firebaseUser)
        {
            Navigation.PushModalAsync(new ApplicationPage(firebaseUser));
        }

        private void OpenMetaInformation(FirebaseUser firebaseUser)
        {
            Navigation.PushModalAsync(new MetaInformationPage(firebaseUser));
        }
    }

}
