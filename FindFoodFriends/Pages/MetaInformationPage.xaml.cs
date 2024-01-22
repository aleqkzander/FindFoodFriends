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
        FirebaseDataFile.Delete();
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
                "Falls du es dir anders überlegst warte ich auf dich :)", "Ok");

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
        DisplayAlert("Clean Eater", "Natürliche sowie unverarbeitete und vollwertige Lebensmittel.", "Ok");
    }

    private void AboutLowCarbButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Low Carb", "Wenig Kohlenhydrate, eiweißhaltige, fetthaltige Lebensmittel.", "Ok");
    }

    private void AboutAyurvedaButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Ayurveda", "Spirituelle Lehre: „Lehre vom Leben“.", "Ok");
    }

    private void AboutBasischButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Basische Ernährung", "Basenreiche Lebensmittel und Vermeidung von Säurebildner.", "Ok");
    }

    private void AboutSlowFoodButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Slow Food", "Sozial / ökologisch, verantwortungsvoll mit Blick auf das Tierwohl.", "Ok");
    }

    private void AboutZuckerfreiButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Zuckerfrei", "Meiden von zugesetztem Zucker jeglicher Art. Süßen nur mit Trockenobst oder ganzen Früchten.", "Ok");
    }

    private void AboutWeissmehlfreiButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Weißmehlfrei", "Meiden von Weißmehl.", "Ok");
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
        DisplayAlert("Fairtrade", "Bestimmte soziale/ökonomische Kriterien wie: Mindestpreis für Erzeuger.", "Ok");
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
        DisplayAlert("Nüsse", "Nüsse sowie Erzeugnisse daraus", "Ok");
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
        string traditionellerVeganer = "Traditioneller-Veganer=" + TraditionellerVeganerCheckBox.IsChecked;
        userReferences.Add(traditionellerVeganer);

        string rohVeganer = "Roh-Veganer=" + RohVeganerCheckBox.IsChecked;
        userReferences.Add(rohVeganer);

        string freeganer = "Freeganer=" + FreeganerCheckBox.IsChecked;
        userReferences.Add(freeganer);

        string cleaneater = "Clean-Eater=" + CleanEaterCheckBox.IsChecked;
        userReferences.Add(cleaneater);

        string ayurveda = "Ayurveda=" + AyurvedaCheckBox.IsChecked;
        userReferences.Add(ayurveda);

        string basisch = "Basisch=" + BasischCheckBox.IsChecked;
        userReferences.Add(basisch);

        string slowFood = "Slow-Food=" + SlowFoodCheckBox.IsChecked;
        userReferences.Add(slowFood);

        string zuckerfrei = "Zuckerfrei=" + ZuckerfreiCheckBox.IsChecked;
        userReferences.Add(zuckerfrei);

        string keinWeißmehl = "Weißmehlfrei=" + WeissmehlfreiCheckBox.IsChecked;
        userReferences.Add(keinWeißmehl);

        string saisonal = "Saisonal=" + SaisonalCheckBox.IsChecked;
        userReferences.Add(saisonal);

        string regional = "Regional=" + RegionalCheckBox.IsChecked;
        userReferences.Add(regional);

        string fairtrade = "Fairtrade=" + FairtradeCheckBox.IsChecked;
        userReferences.Add(fairtrade);

        string unverpackt = "Unverpackt=" + UnverpacktCheckBox.IsChecked;
        userReferences.Add(unverpackt);

        string bio = "Bio=" + BioCheckBox.IsChecked;
        userReferences.Add(bio);

        string keineZusatzstoffe = "Keine-Zusatzstoff=" + KeineZusatzstoffeCheckBox.IsChecked;
        userReferences.Add(keineZusatzstoffe);

        string keinGluten = "Kein-Gluten=" + GlutenCheckBox.IsChecked;
        userReferences.Add(keinGluten);

        string keinSesam = "Kein-Sesam=" + SesamCheckBox.IsChecked;
        userReferences.Add(keinSesam);

        string keinSenf = "Kein-Senf=" + SenfCheckBox.IsChecked;
        userReferences.Add(keinSenf);

        string keineLupine = "Keine-Lupine=" + LupineCheckBox.IsChecked;
        userReferences.Add(keineLupine);

        string keineSulfite = "Keine-Sulfite=" + SulfiteCheckBox.IsChecked;
        userReferences.Add(keineSulfite);

        string keineNüsse = "Keine-Nüsse=" + NuesseCheckBox.IsChecked;
        userReferences.Add(keineNüsse);

        string keinSoja = "Kein-Soja=" + SojaCheckBox.IsChecked;
        userReferences.Add(keinSoja);

        string keinSellerie = "Kein-Sellerie=" + SellerieCheckBox.IsChecked;
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
                FirebaseDataFile.Create(firebaseUserJson);
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