using FindFoodFriends.Firebase;
using FindFoodFriends.Firebase.Objects;
using FindFoodFriends.Firebase.Utility;

namespace FindFoodFriends.Pages;

public partial class MetaInformationPage : ContentPage
{
    private readonly FirebaseUser firebaseUser;
    private UserLocation? userLocation = new();
    private readonly List<string> userReferences = [];

    public MetaInformationPage(FirebaseUser firebaseUser)
	{
		InitializeComponent();
        this.firebaseUser = firebaseUser;
        FirebaseDataFile.DeleteData();
        FirebaseDataFile.DeleteMessage();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await AskForLocation();
    }

    private async Task AskForLocation()
    {
        var locationPermissionStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        if (locationPermissionStatus != PermissionStatus.Granted)
        {
            await DisplayAlert("Info", "Ich brauche deinen Standort, damit ich dich in der List der Benutzer sortieren kann." +
                "Wenn du damit nicht einverstanden bist geht es hier leider nicht weiter... :{\n\n" +
                "Falls du es dir anders �berlegst warte ich auf dich :)", "Ok");

            Environment.Exit(0);
        }

        try
        {
            var location = await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Medium, // or other accuracy level
                Timeout = TimeSpan.FromSeconds(30) // optional timeout
            });

            userLocation = new()
            {
                Latitude = location?.Latitude,
                Longitude = location?.Longitude
            };
        }
        catch
        {
            userLocation = null;
        }
    }

    #region ABOUT SECTION
    private void AboutTraditionellerVeganerButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Traditioneller Veganer", "Keine tierischen Lebensmittel.", "Ok");
    }

    private void AboutRohVeganerButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Roh Veganer", "Pflanzliche Lebensmittel die nicht hitzebehandelt wurden.", "Ok");
    }

    private void AboutFreeganerButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Freeganer", "Weggeworfene, abgelaufene, geschenkte, selbstangebaute, gefundene oder gesammelte Lebensmittel.", "Ok");
    }

    private void AboutCleanEaterButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Clean Eater", "Nat�rliche sowie unverarbeitete und vollwertige Lebensmittel.", "Ok");
    }

    private void AboutLowCarbButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Low Carb", "Wenig Kohlenhydrate, eiwei�haltige, fetthaltige Lebensmittel.", "Ok");
    }

    private void AboutAyurvedaButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Ayurveda", "Spirituelle Lehre: �Lehre vom Leben�.", "Ok");
    }

    private void AboutBasischButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Basische Ern�hrung", "Basenreiche Lebensmittel und Vermeidung von S�urebildner.", "Ok");
    }

    private void AboutSlowFoodButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Slow Food", "Sozial / �kologisch, verantwortungsvoll mit Blick auf das Tierwohl.", "Ok");
    }

    private void AboutZuckerfreiButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Zuckerfrei", "Meiden von zugesetztem Zucker jeglicher Art. S��en nur mit Trockenobst oder ganzen Fr�chten.", "Ok");
    }

    private void AboutWeissmehlfreiButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Wei�mehlfrei", "Meiden von Wei�mehl.", "Ok");
    }

    private void AboutSaisonalButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Saisonal", "Bevorzugt Lebensmittel der jeweiligen Jahreszeit.", "Ok");
    }

    private void AboutRegionalButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Regional", "Bevorzugt Lebensmittel von kleinen Unternehmen mit kurzen Transportwegen.", "Ok");
    }

    private void AboutFairtradeButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Fairtrade", "Bestimmte soziale/�konomische Kriterien wie: Mindestpreis f�r Erzeuger.", "Ok");
    }

    private void AboutUnverpacktButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Unverpackt", "Die Lebensmittel sind im Einkauf nicht verpackt.", "Ok");
    }

    private void AboutBioButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Bio", "Lebensmittel sind aus biologischem Anbau.", "Ok");
    }

    private void AboutKeineZusatzstoffeButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Zusatzstoffe", "Vermeidung von E-Stoffen (E-Nummern in der Zutatenliste).", "Ok");
    }

    private void AboutGlutenButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Gluten", "Kein Weizen, Roggen, Gerste, Hafer, Dinkel oder Kamut.", "Ok");
    }

    private void AboutSesamButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Sesam", "Keine Sesam.", "Ok");
    }

    private void AboutSenfButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Senf", "Keine Senferzeugnisse.", "Ok");
    }

    private void AboutLupineButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Lupine", "Lupine sowie Erzeugisse daraus.", "Ok");
    }

    private void AboutSulfiteButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Sulfite", "Meiden von Wein oder schwefelhaltigem Trockenobst.", "Ok");
    }

    private void AboutNuesseButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("N�sse", "N�sse sowie Erzeugnisse daraus", "Ok");
    }

    private void AboutSojaButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Soja", "Kein Soja", "Ok");
    }

    private void AboutSellerieButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Sellerie", "Kein Sellerie", "Ok");
    }

    #endregion ABOUT SECTION

    private bool UsernameIsProvided(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            DisplayAlert("Hinweis", "Du musst einen Benutzernamen festlegen", "Ok");
            return false;
        }
        else return true;
    }

    private async void SendMetaBtn_Clicked(object sender, EventArgs e)
    {
        if (UsernameIsProvided(UserNameEntry.Text) == false)
        {
            return;
        }

        CollectReferences();
        CreateAndAddMetaDataToUser();
        await ExecuteMetaDataRegistrationRequest();
    }

    private void CollectReferences()
    {
        string traditionellerVeganer = "traditioneller-veganer=" + TraditionellerVeganerCheckBox.IsChecked;
        userReferences.Add(traditionellerVeganer);

        string rohVeganer = "roh-veganer=" + RohVeganerCheckBox.IsChecked;
        userReferences.Add(rohVeganer);

        string freeganer = "freeganer=" + FreeganerCheckBox.IsChecked;
        userReferences.Add(freeganer);

        string cleaneater = "clean-eater=" + CleanEaterCheckBox.IsChecked;
        userReferences.Add(cleaneater);

        string ayurveda = "ayurveda=" + AyurvedaCheckBox.IsChecked;
        userReferences.Add(ayurveda);

        string basisch = "basisch=" + BasischCheckBox.IsChecked;
        userReferences.Add(basisch);

        string slowFood = "slow-food=" + SlowFoodCheckBox.IsChecked;
        userReferences.Add(slowFood);

        string zuckerfrei = "zuckerfrei=" + ZuckerfreiCheckBox.IsChecked;
        userReferences.Add(zuckerfrei);

        string keinWei�mehl = "wei�mehlfrei=" + WeissmehlfreiCheckBox.IsChecked;
        userReferences.Add(keinWei�mehl);

        string saisonal = "saisonal=" + SaisonalCheckBox.IsChecked;
        userReferences.Add(saisonal);

        string regional = "regional=" + RegionalCheckBox.IsChecked;
        userReferences.Add(regional);

        string fairtrade = "fairtrade=" + FairtradeCheckBox.IsChecked;
        userReferences.Add(fairtrade);

        string unverpackt = "unverpackt=" + UnverpacktCheckBox.IsChecked;
        userReferences.Add(unverpackt);

        string bio = "bio=" + BioCheckBox.IsChecked;
        userReferences.Add(bio);

        string keineZusatzstoffe = "keine-zusatzstoff=" + KeineZusatzstoffeCheckBox.IsChecked;
        userReferences.Add(keineZusatzstoffe);

        string keinGluten = "kein-gluten=" + GlutenCheckBox.IsChecked;
        userReferences.Add(keinGluten);

        string keinSesam = "kein-sesam=" + SesamCheckBox.IsChecked;
        userReferences.Add(keinSesam);

        string keinSenf = "kein-senf=" + SenfCheckBox.IsChecked;
        userReferences.Add(keinSenf);

        string keineLupine = "keine-lupine=" + LupineCheckBox.IsChecked;
        userReferences.Add(keineLupine);

        string keineSulfite = "keine-sulfite=" + SulfiteCheckBox.IsChecked;
        userReferences.Add(keineSulfite);

        string keineN�sse = "keine-n�sse=" + NuesseCheckBox.IsChecked;
        userReferences.Add(keineN�sse);

        string keinSoja = "kein-soja=" + SojaCheckBox.IsChecked;
        userReferences.Add(keinSoja);

        string keinSellerie = "kein-sellerie=" + SellerieCheckBox.IsChecked;
        userReferences.Add(keinSellerie);
    }

    private void CreateAndAddMetaDataToUser()
    {
        FirebaseUserMeta firebaseUserMeta = new()
        {
            Name = UserNameEntry.Text,
            References = userReferences,
            Location = userLocation,
        };

        firebaseUser.Meta = firebaseUserMeta;
    }

    private async Task ExecuteMetaDataRegistrationRequest()
    {
        try
        {
            string response = await FirebaseDatabase.AddUserMetaAsync(firebaseUser.UserID!, firebaseUser.Meta!);
            if (response == "success")
            {
                string firebaseUserJson = FirebaseJsonHelper.ConvertFirebaseUserToJsonObject(firebaseUser)!;
                FirebaseDataFile.CreateData(firebaseUserJson);
                await Navigation.PushModalAsync(new ApplicationPage(firebaseUser));
            }
            else
            {
                await DisplayAlert("Fehler", "Da ist etwas schief gelaufen. Versuche es erneut.", "Ok");
                firebaseUser.Meta = null;
            }
        }
        catch
        {
            await DisplayAlert("Error", "Oh das ist etwas schief gelaufen...", "Ok");
            firebaseUser.Meta = null;
        }
    }
}