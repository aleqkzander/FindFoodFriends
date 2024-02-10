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
		GetMessagesFromDatabase();
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
                    if (message.Receiver == scoreuser!.DatabaseUser!.Name)
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
            string? sendingResponse = await FirebaseDatabase.SendMessageToDatabase(firebaseUser.UserID!, firebaseMessage);

            if (sendingResponse == "success")
            {
                ChatView chatView = new(firebaseMessage.Timestamp!, firebaseMessage.Sender!, firebaseMessage.Message!);
				AddChatViewToContainer(chatView);
            }
			else
			{
                await DisplayAlert("Error", "Es ist ein unbekannter Fehler aufgetreten. Wende dich bitte an einen Administrator", "Ok");
            }
        }
		catch
		{
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