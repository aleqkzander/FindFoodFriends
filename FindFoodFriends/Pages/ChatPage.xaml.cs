/*
 * The userid will be utilized for authentication
 */

using FindFoodFriends.Firebase;
using FindFoodFriends.Firebase.Objects;
namespace FindFoodFriends.Pages;

public partial class ChatPage : ContentPage
{
	private readonly FirebaseUser firebaseUser;
	private readonly ScoreUser scoreuser;
    private bool isListening = false;
    private readonly HashSet<string> displayedMessageIds = [];


    public ChatPage(FirebaseUser firebaseUser, ScoreUser scoreuser)
	{
		InitializeComponent();
		this.firebaseUser = firebaseUser;
		this.scoreuser = scoreuser;
		Chatuser.Text = scoreuser.DatabaseUser!.Name;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        isListening = true;

        while (isListening)
        {
            await ListenForDatabaseChanges();
            await Task.Delay(TimeSpan.FromSeconds(5000)); // Every 5 seconds
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        isListening = false;
    }

	private async Task ListenForDatabaseChanges()
	{
		try
		{
			while (true)
			{
                using HttpClient client = new();
                List<FirebaseMessage>? messageList = await FirebaseDatabase.ListenForDatabaseChanges(client, firebaseUser.UserID!);

                if (messageList?.Count != 0)
                {
                    foreach (FirebaseMessage message in messageList!)
                    {
                        if (!displayedMessageIds.Contains(message.MessageId!))
                        {
                            if (message.Receiver == scoreuser!.DatabaseUser!.Name || message.Sender == scoreuser!.DatabaseUser!.Name)
                            {
                                ChatView chatView = new(message.Timestamp!, message.Sender!, message.Message!);
                                AddChatViewToContainer(chatView);
                                displayedMessageIds.Add(message.MessageId!);
                            }
                        }
                    }
                }
            }
		}
		catch
		{

		}
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
            FirebaseMessage firebaseMessage = new(firebaseUser!.Meta!.Name!, scoreuser!.DatabaseUser!.Name!, message, DateTime.Now);
            string? sendingResponse = await FirebaseDatabase.SendMessageToDatabase(firebaseUser.UserID!, scoreuser.DatabaseUser.UserId!,firebaseMessage);

            if (sendingResponse == "success") {}
			else
			{
                await DisplayAlert("Error", "Oh das ist etwas schief gelaufen...", "Ok");
            }
        }
		catch
		{
            await DisplayAlert("Error", "Oh das ist etwas schief gelaufen...", "Ok");
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


