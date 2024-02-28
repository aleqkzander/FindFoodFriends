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
            await Dispatcher.DispatchAsync(EnableLoadingAnimation);
            LoadLocalData();
        }

        private void LoadLocalData()
        {
            if (FirebaseDataFile.IsPresentDataFile() == false)
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
                FirebaseUser? localFirebaseUser = FirebaseJsonHelper.ConvertJsonObjectToFirebaseUser(FirebaseDataFile.GetData());
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
                    FirebaseUser? localFirebaseUser = FirebaseJsonHelper.ConvertJsonObjectToFirebaseUser(FirebaseDataFile.GetData());
                    if (firebaseUser.Meta != null) OpenMainApplication(firebaseUser);
                    else OpenMetaInformation(firebaseUser);
                }
            }
            catch
            {
                await DisplayAlert("Error", "Oh das ist etwas schief gelaufen...", "Ok");
            }
        }

        private async Task EnableLoadingAnimation()
        {
            await Task.Delay(1);
            loading.IsAnimationPlaying = true;
        }

        private void DisableLoadingAnimation()
        {
            loading.IsAnimationPlaying = false;
            loading.IsVisible = false;
        }

        private void OpenWelcome()
        {
            DisableLoadingAnimation();
            Navigation.PushModalAsync(new WelcomePage());
        }

        private void OpenMainApplication(FirebaseUser firebaseUser)
        {
            DisableLoadingAnimation();
            Navigation.PushModalAsync(new ApplicationPage(firebaseUser));
        }

        private void OpenMetaInformation(FirebaseUser firebaseUser)
        {
            DisableLoadingAnimation();
            Navigation.PushModalAsync(new MetaInformationPage(firebaseUser));
        }
    }

}
