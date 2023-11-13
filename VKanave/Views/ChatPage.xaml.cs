using VKanave.Models;

namespace VKanave.Views;

public partial class ChatPage : ContentPage
{
	public ChatPage(ChatModel chat)
	{
		InitializeComponent();
		_user = chat.User;
	}

	private UserModel _user;

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
		Navigation.PopModalAsync();
    }
}