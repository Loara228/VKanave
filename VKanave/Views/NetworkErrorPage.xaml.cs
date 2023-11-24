namespace VKanave.Views;

public partial class NetworkErrorPage : ContentPage
{
	public NetworkErrorPage(string message)
	{
		InitializeComponent();
		labelError.Text = message;
	}
}