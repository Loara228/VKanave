using VKanave.Views;

namespace VKanave;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        _current = this;

    }

    public static void UpdateLocalInfo(string username)
    {
        _current.labelUsername.Text = username;
    }

    private static AppShell _current;
}
