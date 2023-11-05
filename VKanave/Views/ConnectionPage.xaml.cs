using VKanave.Networking;

namespace VKanave.Views;

public partial class ConnectionPage : ContentPage
{
    public ConnectionPage()
    {
        InitializeComponent();
        Loaded += async (s, e) =>
        {
            if (MauiProgram.DebugCode == 1)
                await Continue();
            else
                Initialize();
        };
    }

    protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        if (!string.IsNullOrEmpty(MauiProgram.Token))
        {
            await Navigation.PopModalAsync();
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
}