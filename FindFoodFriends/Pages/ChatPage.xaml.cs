/*
 * The userid will be utilized for authentication
 * Use streaming to listen for changes in realtime and call an event
 * https://firebase.google.com/docs/database/rest/retrieve-data#section-rest-streaming
 */

using FindFoodFriends.Firebase;
using FindFoodFriends.Firebase.Objects;
using System.Net;
namespace FindFoodFriends.Pages;

public partial class ChatPage : ContentPage
{
	private readonly FirebaseUser firebaseUser;
	private readonly ScoreUser scoreuser;
    private readonly List<FirebaseMessage> initialUserMessages;
    private readonly HashSet<string> displayedMessageIds = [];

    public ChatPage(FirebaseUser firebaseUser, ScoreUser scoreuser, List<FirebaseMessage> initialUserMessages)
	{
		InitializeComponent();
		this.firebaseUser = firebaseUser;
		this.scoreuser = scoreuser;
		Chatuser.Text = scoreuser.DatabaseUser!.Name;
        this.initialUserMessages = initialUserMessages;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Dispatcher.DispatchAsync(EnableLoadingAnimation);
        AssignAndDisplayMessagesForUser(scoreuser);

        string location = $"messages/{firebaseUser.UserID!.LocalId}";
        await ListenForDatabaseChanges(location);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

    private void AssignAndDisplayMessagesForUser(ScoreUser user)
    {
        try
        {
            if (initialUserMessages?.Count != 0)
            {
                foreach (FirebaseMessage message in initialUserMessages!)
                {
                    if (!displayedMessageIds.Contains(message.MessageId!))
                    {
                        if (message.Receiver == user!.DatabaseUser!.Name || message.Sender == user!.DatabaseUser!.Name)
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
    /// List for database changes at specified location
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    private async Task ListenForDatabaseChanges(string location)
    {
        /*
         * Dispose the client automaticly when not required anymore
         */

        try
        {
            using var client = new HttpClient();
            var requestUri = $"{FirebaseEndpoints.DatabaseEndpoint}/{location}.json?auth={firebaseUser.UserID!.IdToken}";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/event-stream"));
            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (response.IsSuccessStatusCode)
            {
                await ProcessEventStream(response);
            }
            else if (response.StatusCode == HttpStatusCode.Redirect || response.StatusCode == HttpStatusCode.TemporaryRedirect)
            {
                await HandleRedirect(response);
            }
            else
            {
                client.Dispose();
                throw new HttpRequestException($"Failed to stream changes. Status code: {response.StatusCode}");
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// While listening on database changes handle the event stream
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    private async Task ProcessEventStream(HttpResponseMessage response)
    {
        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
        {
            await DownloadAndDisplayMissingMessages();
        }
    }

    /// <summary>
    /// While listening on database changes handler the redirect
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    private async Task HandleRedirect(HttpResponseMessage response)
    {
        var redirectedLocation = response.Headers.Location?.ToString();
        if (!string.IsNullOrEmpty(redirectedLocation))
        {
            // call ListenForDatabaseChanges recursively with the new location
            await ListenForDatabaseChanges(redirectedLocation);
        }
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
            AssignAndDisplayMessagesForUser(scoreuser);
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
    /// Download new messages as soon as something has changed
    /// </summary>
    /// <returns></returns>
    private async Task DownloadAndDisplayMissingMessages() 
    {
        try
        {
            using HttpClient client = new();
            int messagesCount = await FirebaseDatabase.CountMessagesForUser(client, firebaseUser.UserID!);

            if (displayedMessageIds.Count < messagesCount)
            {
                int amountOfMessagesToDownload = messagesCount - displayedMessageIds.Count;
                List<FirebaseMessage>? newDownloadedMessages = await FirebaseDatabase.DownloadAmountOfMessages(client, firebaseUser.UserID!, amountOfMessagesToDownload);

                if (newDownloadedMessages?.Count != 0)
                {
                    foreach (FirebaseMessage message in newDownloadedMessages!)
                    {
                        // we keep track of all messages
                        initialUserMessages.Add(message);

                        // but display messages only for the specific user
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
        await Task.Delay(1);
    }

    private void DisableLoadingAnimation()
    {
        loading.IsAnimationPlaying = false;
        loading.IsVisible = false;
    }

    private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
    {
        var navigationStack = Navigation.NavigationStack;
        if (navigationStack.Count > 0)
        {
            var previousPage = navigationStack[navigationStack.Count - 1] as ApplicationPage;
            previousPage?.UpdateInitialUserMessages(initialUserMessages);
        }
    }
}