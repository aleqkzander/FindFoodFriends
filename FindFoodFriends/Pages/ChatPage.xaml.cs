/*
 * The userid will be utilized for authentication
 * Use streaming to listen for changes in realtime and call an event
 * https://firebase.google.com/docs/database/rest/retrieve-data#section-rest-streaming
 */

using FindFoodFriends.Firebase;
using FindFoodFriends.Firebase.Objects;
namespace FindFoodFriends.Pages;

public partial class ChatPage : ContentPage
{
	private readonly FirebaseUser firebaseUser;
	private readonly ScoreUser scoreuser;
    private readonly HashSet<string> displayedMessageIds = [];
    private bool isListening;

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
        await PollForDatabaseChanges(isListening);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        isListening = false;
    }

    /// <summary>
    /// Start listening for message differences every 5 seconds
    /// </summary>
    /// <param name="listening"></param>
    /// <returns></returns>
    private async Task PollForDatabaseChanges(bool listening)
    {
        try
        {
            await Dispatcher.DispatchAsync(EnableLoadingAnimation);
            await DownloadAndDisplayAllMessagesForReceiver();
        }
        catch
        {

        }

        while (listening)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                await DownloadAndDisplayMissingMessages();
            }
            catch
            {
            }
        }
    }

    /// <summary>
    /// Call method to download messages at the begining
    /// </summary>
    /// <returns></returns>
    private async Task DownloadAndDisplayAllMessagesForReceiver()
    {
        try
        {
            using HttpClient client = new();
            List<FirebaseMessage>? messageList = await FirebaseDatabase.DownloadAllMessages(client, firebaseUser.UserID!);

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

            DisableLoadingAnimation();
        }
        catch
        {
        }
    }

    /// <summary>
    /// Get count of messages
    /// </summary>
    /// <returns></returns>
    private async Task<int> GetMessageCount()
    {
        try
        {
            using HttpClient client = new();
            int messagesCount = await FirebaseDatabase.CountMessagesForUser(client, firebaseUser.UserID!);
            return messagesCount;
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// Download missing messages
    /// </summary>
    /// <returns></returns>
    private async Task DownloadAndDisplayMissingMessages() 
    {
        try
        {
            int messagesCount = await GetMessageCount();
            if (displayedMessageIds.Count < messagesCount)
            {
                int amountOfMessagesToDownload = messagesCount - displayedMessageIds.Count;
                using HttpClient client = new();
                List<FirebaseMessage>? messageList = await FirebaseDatabase.DownloadAmountOfMessages(client, firebaseUser.UserID!, amountOfMessagesToDownload);

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
            SendBtn.IsEnabled = false;
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
                MessageEntry.Text = string.Empty;
                await DownloadAndDisplayMissingMessages();
            }

            SendBtn.IsEnabled = true;
        }
		catch
		{
            SendBtn.IsEnabled = true;
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
}


