using VKanave.Views;

namespace VKanave;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new LoginPage();
	}

    protected override Window CreateWindow(IActivationState activationState)
    {
        Window window = base.CreateWindow(activationState);

		window.Width = 450;
		window.Height = 800;

		return window;
    }
}
