using System.Collections.ObjectModel;
using VKanave.Models;
using VKanave.Networking;
using VKanave.Networking.NetMessages;
using VKanave.Extensions;

namespace VKanave.Views;

public partial class ChatPage : ContentPage
{
    public ChatPage(ChatModel chat)
    {
        InitializeComponent();
        Current = this;
        _user = chat.User;
        labelUsername.Text = chat.User.Username;
        labelLastActive.Text = GetLastSeenTitle(chat.User.unixTime);

        messagesList.ItemsSource = Messages;

        Loaded += (s, e) => Networking.Networking.Send(new NMLoadMessages() { localUserId = LocalUser.Id, userId2 = _user.ID });

        messagesList.ItemTemplate = new DataTemplate(() =>
        {
            Frame messageFrame = new Frame();
            StackLayout messageLayout = new StackLayout();
            Label messageText = new Label();
            Label dateTimeLabel = new Label();

            messageFrame.Margin = 10;
            messageFrame.Padding = 20;
            messageFrame.CornerRadius = 20;
            messageFrame.BorderColor = Colors.Transparent;

            messageText.TextColor = Colors.White;

            dateTimeLabel.TextColor = Colors.Gray;
            dateTimeLabel.FontSize = 10;
            dateTimeLabel.HorizontalOptions = LayoutOptions.EndAndExpand;

            messageFrame.SetBinding(StackLayout.BackgroundColorProperty, new Binding("BackgroundColor"));
            messageFrame.SetBinding(StackLayout.MarginProperty, new Binding("Margin"));
            messageText.SetBinding(Label.TextProperty, new Binding("Content"));
            dateTimeLabel.SetBinding(Label.TextProperty, new Binding("DateTime"));
            dateTimeLabel.SetBinding(Label.TextColorProperty, new Binding("DateTimeColor"));

            messageFrame.Content = messageLayout;
            messageLayout.Add(messageText);
            messageLayout.Add(dateTimeLabel);

            return messageFrame;
        });
    }

    private string GetLastSeenTitle(int unixTime)
    {
        DateTime dt = unixTime.ToDateTime();
        return $"Last seen at " + dt.ToString();
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    public static ChatPage Current
    {
        get; private set;
    }

    public ObservableCollection<MessageModel> Messages
    {
        get; set;
    } = new ObservableCollection<MessageModel>();

    private UserModel _user;
}