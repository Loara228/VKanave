using CommunityToolkit.Maui.Views;

namespace VKanave.Views.Popups;

public partial class TextboxPopup : Popup
{
	public TextboxPopup(string title, string buttonText = "OK", Action<string> callback = null)
	{
		InitializeComponent();
		labelTitle.Text = title;
		button1.Text = buttonText;
		this.callback = callback;
	}

	public TextboxPopup()
	{
		InitializeComponent();
		labelTitle.Text = "?";
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		cancel = false;

		if (textbox1.Text != null)
			result = textbox1.Text;

		callback?.Invoke(result);

		Close();
    }

	public Action<string> callback;
	public bool cancel = true;
	private string result = string.Empty;

}