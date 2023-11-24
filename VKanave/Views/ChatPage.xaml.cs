using System.Collections.ObjectModel;
using VKanave.Models;
using VKanave.Networking;
using VKanave.Networking.NetMessages;
using VKanave.Extensions;
using VKanave.Networking.NetObjects;
using VKanave.Networking.NetMessages.NMCMFlags;
using CommunityToolkit.Maui.Alerts;

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

        richTextbox.Completed += SendChatMessage;

#if ANDROID
        Messages.CollectionChanged += (s, e) =>
        {
            ScrollToEnd();
        };
#endif

        Loaded += (s, e) => Networking.Networking.Send(new NMLoadMessages() { localUserId = LocalUser.Id, userId2 = _user.ID });

        messagesList.ItemTemplate = new DataTemplate(() =>
        {
            Frame messageFrame = new Frame();
            StackLayout messageLayout = new StackLayout();
            Label messageText = new Label();
            Label dateTimeLabel = new Label();
            Label unreadLabel = new Label();

            messageFrame.Margin = 50;
            messageFrame.Padding = 10;
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
            messageText.SetBinding(Label.TextColorProperty, new Binding("TextColor"));

            dateTimeLabel.SetBinding(Label.TextProperty, new Binding("DateTime"));
            dateTimeLabel.SetBinding(Label.TextColorProperty, new Binding("DateTimeColor"));

            unreadLabel.SetBinding(Label.IsVisibleProperty, new Binding("Unread", BindingMode.TwoWay));
            unreadLabel.SetBinding(Label.TextProperty, new Binding("UnreadText", BindingMode.TwoWay));

            messageFrame.Content = messageLayout;
            messageLayout.Add(messageText);
            messageLayout.Add(unreadLabel);
            messageLayout.Add(dateTimeLabel);

            TapGestureRecognizer tapRecognizer = new TapGestureRecognizer(); tapRecognizer.Tapped += async (s, e) =>
            {
                MessageModel chatMsg = (MessageModel)((s as View).BindingContext);
                await OnMessageTapped(chatMsg, e);
            };
            messageFrame.GestureRecognizers.Add(tapRecognizer);

            return messageFrame;
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Current.User = null;
        Networking.Networking.Send(new NMChats() { localUserId = LocalUser.Id });
    }

    private async Task OnMessageTapped(MessageModel chatMsg, TappedEventArgs args)
    {
        if (chatMsg.SystemMessage)
            return;
        string result = await DisplayActionSheet("ActionSheet", "cancel", null, "Copy", "Delete");
        if (result == "Delete")
        {
            Messages.Remove(chatMsg);
            if (!chatMsg.Local)
                await Toast.Make("Message deleted locally").Show();
            else
                Networking.Networking.Send(new NMDelete() { messageId = chatMsg.ID, dialogId = User.ID });
        }
        else if (result == "Copy")
        {
            await Clipboard.SetTextAsync(chatMsg.Content);
        }
    }

    private void SendChatMessage(object sender, EventArgs e)
    {
        string? message = richTextbox.Text;
        if (message != null && message.Length > 0)
        {
            richTextbox.Text = "";
            NMChatMessage msg = new NMChatMessage()
            {
                ChatMessage =
                new ChatMessage(
                    new ChatUser(User.ID, User.Username, 0),
                    0,
                    message,
                    DateTime.UtcNow.ToUnixTime(),
                    (int)ChatMessageFlags.UNREAD)
            };
            msg.id_from = LocalUser.Id;
            msg.id_to = User.ID;
            Networking.Networking.Send(msg);
        }
    }

    private string GetLastSeenTitle(int unixTime)
    {
        DateTime dt = unixTime.ToDateTime();
        if (DateTime.Now < dt.AddMinutes(2))
            return "Online";
        return $"Last seen at " + dt.ToString();
    }

    public void ScrollToEnd()
    {
        if (Messages.Count > 0)
            messagesList.ScrollTo(Messages.Count - 1);
    }

    public static ChatPage Current
    {
        get; private set;
    }

    public UserModel User
    {
        get => _user;
        set => _user = value;
    }

    public ObservableCollection<MessageModel> Messages
    {
        get; set;
    } = new ObservableCollection<MessageModel>();

    private UserModel _user;

}