namespace FindFoodFriends.Pages;

public partial class ChatView : ContentView
{
	public ChatView(string timestamp, string sender, string message)
	{
		InitializeComponent();
		TimestampLabel.Text = timestamp;
        SenderLabel.Text = "von " + sender;
		MessageLabel.Text = message;
	}

	public Label GetTimestampLabel()
	{
		return TimestampLabel;
	}
}