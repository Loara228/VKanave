using VKanave.Networking;

namespace VKanave.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		Connection.InitializeLocal();
		Connection.Current.Connect();
	}

    private void Button_SignIn_Clicked(object sender, EventArgs e)
    {
		Networking.Networking.SendData("Hello World");
    }
}