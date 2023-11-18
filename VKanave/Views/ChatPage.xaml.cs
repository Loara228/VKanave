using System.Collections.ObjectModel;
using VKanave.Models;
using VKanave.Networking;
using VKanave.Networking.NetMessages;
using VKanave.Extensions;
using VKanave.Networking.NetObjects;

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
            Label unreadLabel = new Label();

            messageFrame.Margin = 10;
            messageFrame.Padding = 20;
            messageFrame.CornerRadius = 20;
            messageFrame.BorderColor = Colors.Transparent;

            messageText.TextColor = Colors.White;

            dateTimeLabel.TextColor = Colors.Gray;
            dateTimeLabel.FontSize = 10;
            dateTimeLabel.HorizontalOptions = LayoutOptions.EndAndExpand;

            unreadLabel.TextColor = Color.FromArgb("#e86d6d");
            unreadLabel.FontSize = 10;
            unreadLabel.HorizontalOptions = LayoutOptions.End;
            unreadLabel.Text = "unread";

            messageFrame.SetBinding(StackLayout.BackgroundColorProperty, new Binding("BackgroundColor"));
            messageFrame.SetBinding(StackLayout.MarginProperty, new Binding("Margin"));

            messageText.SetBinding(Label.TextProperty, new Binding("Content"));

            dateTimeLabel.SetBinding(Label.TextProperty, new Binding("DateTime"));
            dateTimeLabel.SetBinding(Label.TextColorProperty, new Binding("DateTimeColor"));

            unreadLabel.SetBinding(Label.IsVisibleProperty, new Binding("Unread"));
            unreadLabel.SetBinding(Label.TextProperty, new Binding("UnreadText"));

            messageFrame.Content = messageLayout;
            messageLayout.Add(messageText);
            messageLayout.Add(unreadLabel);
            messageLayout.Add(dateTimeLabel);

            return messageFrame;
        });
    }

    private void SendChatMessage(object sender, EventArgs e)
    {
        string message = richTextbox.Text;
        if (message.Length > 0)
        {
            richTextbox.Text = "";
            NMChatMessage msg = new NMChatMessage()
            {
                ChatMessage =
                new ChatMessage(
                    new ChatUser(User.ID, User.Username, 0),
                    0,
                    message,
                    0,
                    0)
            };
            msg.id_from = LocalUser.Id;
            msg.id_to = User.ID;
            System.Diagnostics.Debug.WriteLine($"message sent. from: {LocalUser.Id}. to: {User.ID}");
            Networking.Networking.Send(msg);
        }
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

    private void AddMessage(MessageModel message)
    {
        Messages.Add(message);
    }

    public static ChatPage Current
    {
        get; private set;
    }

    public UserModel User
    {
        get => _user;
    }

    public ObservableCollection<MessageModel> Messages
    {
        get; set;
    } = new ObservableCollection<MessageModel>();

    private UserModel _user;
}