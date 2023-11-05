using VKanave.Networking;
using VKanave.Networking.NetMessages;

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
        Networking.Networking.SendData(new NMAuth(textboxUsername.Text, textboxUsername.Text));
    }

    public void SignIn(string result)
    {
        MainThread.BeginInvokeOnMainThread(() => {
            DisplayAlert("token", result.ToString(), "OK");
        });

    }
}