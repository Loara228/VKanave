using VKanave.Networking;
using VKanave.Networking.NetMessages;

namespace VKanave.Views;

public partial class ConnectionPage : ContentPage
{
    public ConnectionPage()
    {
        InitializeComponent();
        Loaded += async (s, e) =>
        {
            if (MauiProgram.DebugCode == 1 || MauiProgram.DebugCode == 2)
                await Continue();
            else
                Initialize();
        };
    }

    protected override /*async*/ void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        if (!string.IsNullOrEmpty(LocalUser.Token))
        {
            activityIndicator1.IsRunning = false;
            button1.IsVisible = true;
            //await Navigation.PopModalAsync();
        }
    }

    private async Task Initialize()
    {
        await Task.Delay(500);
        Connection.InitializeLocal();
        if (Connection.Current.Connect())
            await Continue();
        else
        {
            activityIndicator1.IsRunning = false;
            var result = await DisplayAlert("Blya :(", "Failed to connect to the server", "Retry", "Cancel");
            if (result)
            {
                activityIndicator1.IsRunning = true;
                await Initialize();
            }
            else
            {
                Application.Current.Quit();
            }
        }
    }

    private async Task Continue()
    {
        await Navigation.PushModalAsync(new LoginPage());
    }

    private void button1_Clicked(object sender, EventArgs e)
    {
        if (MauiProgram.DebugCode == 0)
            Networking.Networking.Send(new NMChats() { localUserId = LocalUser.Id });
        Navigation.PopModalAsync();
    }
}