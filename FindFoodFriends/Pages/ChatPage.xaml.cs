using FindFoodFriends.Firebase.Objects;

namespace FindFoodFriends.Pages;

public partial class ChatPage : ContentPage
{
	private readonly ScoreUser scoreuser;

	public ChatPage(ScoreUser scoreuser)
	{
		InitializeComponent();
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

	private void SendMessage(string message)
	{
		FirebaseMessage firebaseMessage = new(scoreuser!.LocalUser!.Name!, message, DateTime.Now);
		ChatView chatView = new(firebaseMessage.Timestamp, firebaseMessage.Sender, firebaseMessage.Message);
        AddChatViewToContainer(chatView);

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