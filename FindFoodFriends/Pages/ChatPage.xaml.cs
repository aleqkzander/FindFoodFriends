using FindFoodFriends.Firebase.Objects;

namespace FindFoodFriends.Pages;

public partial class ChatPage : ContentPage
{
	private readonly ScoreUser scoreuser;

	public ChatPage(ScoreUser scoreuser)
	{
		InitializeComponent();
		this.scoreuser = scoreuser;
	}
}