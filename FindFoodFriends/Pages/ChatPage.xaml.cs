/*
 * The userid will be utilized for authentication
 */

using FindFoodFriends.Firebase;
using FindFoodFriends.Firebase.Objects;
namespace FindFoodFriends.Pages;

public partial class ChatPage : ContentPage
{
	private readonly FirebaseUserID firebaseUserID;
	private readonly ScoreUser scoreuser;

	public ChatPage(FirebaseUserID firebaseUserID, ScoreUser scoreuser)
	{
		InitializeComponent();
		this.firebaseUserID = firebaseUserID;
		this.scoreuser = scoreuser;
		Chatuser.Text = scoreuser.DatabaseUser!.Name;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		GetMessagesFromDatabase();
    }

    private void GetMessagesFromDatabase()
	{
		/*
		 * Access the firebase database and download all messages 
		 */

		ScrollToTheBottom();
    }

    private void SendBtn_Clicked(object sender, EventArgs e)
    {
		if (!string.IsNullOrEmpty(MessageEntry.Text))
		{
            SendMessage(MessageEntry.Text);
        }
    }

	private async void SendMessage(string message)
	{
		try
		{
			await DisplayAlert("Info", $"Send message: {message}", "Ok");
            FirebaseMessage firebaseMessage = new(scoreuser!.LocalUser!.Name!, scoreuser!.DatabaseUser!.Name!, message, DateTime.Now);
            string sendingResponse = await FirebaseDatabase.SendMessageToDatabase(firebaseUserID, firebaseMessage);

            if (sendingResponse == "success")
            {
				// The message was send to the database. From here on do we add localy or do we pull from server?
                ChatView chatView = new(firebaseMessage.Timestamp, firebaseMessage.Sender, firebaseMessage.Message);
				AddChatViewToContainer(chatView);
            }
			else
			{
                await DisplayAlert("Error", "Es ist ein unbekannter Fehler aufgetreten. Wende dich bitte an einen Administrator", "Ok");
            }
        }
		catch (Exception exception)
		{
            //await DisplayAlert("Error", exception.ToString(), "Ok");
            await DisplayAlert("Error", "Es ist ein unbekannter Fehler aufgetreten. Wende dich bitte an einen Administrator", "Ok");
        }
    }

	private void AddChatViewToContainer(ChatView chatview)
	{
        MessageEntry.Text = string.Empty;
        MessageContainer.Children.Add(chatview);
		ScrollToTheBottom();
    }

	private void ScrollToTheBottom()
	{
        ScrollView.ScrollToAsync(0, ScrollView.Height + 9999, true);
    }
}