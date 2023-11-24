using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using VKanave.Networking;
using VKanave.Networking.NetMessages;
using VKanave.Networking.NetObjects;

namespace VKanave.Views.Popups;

public partial class AddUserPopup : Popup
{
	public AddUserPopup()
	{
		InitializeComponent();
        textbox1.TextChanged += (s, e) =>
        {
            frame1.BorderColor = Colors.Transparent;
            labelResult.IsVisible = false;
        };
        Current = this;
        CanBeDismissedByTappingOutsideOfPopup = false;
    }

    public void DisplayResult(NewChatResult result)
    {
        MarkTextbox(result == NewChatResult.SUCCESS);
        labelResult.IsVisible = true;
        if (result == NewChatResult.SUCCESS)
            labelResult.Text = "Enjoy chatting!";
        else if (result == NewChatResult.ALREADY_EXISTS)
            labelResult.Text = "The chat already exists";
        else if (result == NewChatResult.USER_NOT_FOUND)
            labelResult.Text = "User not found";
        _canClose = true;
    }

    private void MarkTextbox(bool success)
    {
        frame1.BorderColor = success ? Colors.Lime : Colors.Red;
        labelResult.TextColor = success ? Colors.Lime : Colors.Red;
    }

    //Send first message
    private void Button_Clicked(object sender, EventArgs e)
    {
        _canClose = false;
        if (textbox1.Text != null && !string.IsNullOrWhiteSpace(textbox1.Text))
        {
            string username = textbox1.Text;
            if (username == LocalUser.Username)
            {
                Toast.Make("nope :)").Show();
                return;
            }
            Networking.Networking.Send(new NMNewChat() { username = username });
            return;
        }
        _canClose = true;
        Toast.Make("Incorrect format!").Show();
    }

    //"Close" button pressed
    private void Button_Clicked_1(object sender, EventArgs e)
    {
        if (_canClose)
            Close();
    }

    public static AddUserPopup Current
    {
        get; private set;
    }

    private bool _canClose = true;

}