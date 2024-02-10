/*
 * The userid will be utilized for authentication
 */

using FindFoodFriends.Firebase;
using FindFoodFriends.Firebase.Objects;
using System.Linq;
namespace FindFoodFriends.Pages;

public partial class ChatPage : ContentPage
{
	private readonly FirebaseUser firebaseUser;
	private readonly ScoreUser scoreuser;
	private bool startLisitening;

	public ChatPage(FirebaseUser firebaseUser, ScoreUser scoreuser)
	{
		InitializeComponent();
		this.firebaseUser = firebaseUser;
		this.scoreuser = scoreuser;
		Chatuser.Text = scoreuser.DatabaseUser!.Name;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        startLisitening = true;
        ListenForDatabaseChanges();
        //GetMessagesFromDatabase();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        startLisitening = false;
    }

    private async void GetMessagesFromDatabase()
	{
		try
		{
			List<FirebaseMessage>? messageList = await FirebaseDatabase.GetMessagesFromDatabase(firebaseUser.UserID!);
			if (messageList?.Count == 0)
			{
				return;
			}
			else
			{
				foreach (FirebaseMessage message in messageList!)
				{
                    // only add the designated messages for the user
                    if (message.Receiver == scoreuser!.DatabaseUser!.Name || message.Sender == scoreuser!.DatabaseUser!.Name)
					{
                        ChatView chatView = new(message.Timestamp!, message.Sender!, message.Message!);
                        AddChatViewToContainer(chatView);
                    }
                }
			}

            ScrollToTheBottom();
        }
		catch (Exception exception)
		{
            await DisplayAlert("Error", exception.Message, "Ok");
        }
    }

	private async void ListenForDatabaseChanges()
	{
		try
		{
			while (startLisitening)
			{
                using HttpClient client = FirebaseClient.Instance.GetClient();
                List<FirebaseMessage>? messageList = await FirebaseDatabase.GetMessagesFromDatabase(firebaseUser.UserID!);

                if (messageList?.Count != 0)
                {
                    foreach (FirebaseMessage message in messageList!)
                    {
                        if (message.Receiver == scoreuser!.DatabaseUser!.Name || message.Sender == scoreuser!.DatabaseUser!.Name)
                        {
                            ChatView chatView = new(message.Timestamp!, message.Sender!, message.Message!);
                            if (!MessageContainer.Children.Contains(chatView))
                            {
                                // add the message to the container when it's not already present
                                AddChatViewToContainer(chatView);
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

            if (sendingResponse == "success")
            {
                ChatView chatView = new(firebaseMessage.Timestamp!, firebaseMessage.Sender!, firebaseMessage.Message!);
				AddChatViewToContainer(chatView);
            }
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


