namespace VKanave.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        Loaded += async (s, e) =>
        {
            await Navigation.PushModalAsync(new ConnectionPage());
        };
    }
}