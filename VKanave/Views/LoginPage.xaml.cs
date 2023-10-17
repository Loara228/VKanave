namespace VKanave.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		buttonAuth.Clicked += async (s, e) =>
		{
			await DisplayAlert("", "На маму свою кликни", "ok");
		};
	}
}