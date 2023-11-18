using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VKanave.Extensions;
using VKanave.Models;
using VKanave.Networking;
using VKanave.Networking.NetMessages;
using VKanave.Networking.NetObjects;

namespace VKanave.Views;

public partial class ChatsPage : ContentPage
{
    public ChatsPage()
    {
        InitializeComponent();

        Current = this;

        chatList.ItemTemplate = new DataTemplate(() =>
         {
             StackLayout layout = new StackLayout();
             StackLayout layout2 = new StackLayout();
             StackLayout layout3 = new StackLayout(); //unread
             AvatarView avatar = new AvatarView();
             BoxView unread = new BoxView();
             Label unreadLabel = new Label();

             layout3.Orientation = StackOrientation.Horizontal;
             layout3.HorizontalOptions = LayoutOptions.EndAndExpand;
             layout3.Spacing = 5;

             unread.WidthRequest = 16;
             unread.HeightRequest = 16;
             unread.Color = Color.FromArgb("#e86d6d");
             unread.VerticalOptions = LayoutOptions.StartAndExpand;
             unread.CornerRadius = 16;
             unread.SetBinding(BoxView.IsVisibleProperty, new Binding("LastMessage.Unread"));

             unreadLabel.FontSize = 10;
             unreadLabel.TextColor = Color.FromArgb("#e86d6d");
             unreadLabel.VerticalOptions = LayoutOptions.EndAndExpand;
             unreadLabel.Text = "unread";
             unreadLabel.SetBinding(BoxView.IsVisibleProperty, new Binding("LastMessage.Unread"));

             avatar.ImageSource = ImageSource.FromFile("avatar.png");
             avatar.Text = "null";
             avatar.BorderWidth = 0;
             avatar.Margin = 10;

             layout.Padding = 5;
             layout.BackgroundColor = Color.FromArgb($"#272938");
             layout.HorizontalOptions = LayoutOptions.FillAndExpand;

             layout.Background = new LinearGradientBrush(new GradientStopCollection()
             {
                 new GradientStop(Color.FromArgb("#1A1A26"), 0.0f),
                 new GradientStop(Color.FromArgb("#272938"), 1.0f)
             }, new Point(0, 0), new Point(1, 1));

             layout2.Orientation = StackOrientation.Horizontal;
             layout2.HorizontalOptions = LayoutOptions.FillAndExpand;
             layout2.Margin = new Thickness(0, 1);

             Label labelUsername = new Label { FontSize = 16, TextColor = Color.FromArgb("#879EEC"), FontAttributes = FontAttributes.Bold };
             labelUsername.SetBinding(Label.TextProperty, new Binding("User.Username"));

             Label labelLastMessage = new Label { FontSize = 14, TextColor = Colors.Silver };
             labelLastMessage.SetBinding(Label.TextProperty, new Binding("LastMessage.Preview"));

             Label labelDate = new Label { FontSize = 10, TextColor = Colors.Gray };
             labelDate.SetBinding(Label.TextProperty, new Binding("LastMessage.DateTime"));
             labelDate.HorizontalOptions = LayoutOptions.EndAndExpand;


             layout3.Add(unreadLabel);
             layout3.Add(unread);

             layout.Add(labelUsername);
             layout.Add(labelLastMessage);
             layout.Add(layout3);
             layout.Add(labelDate);

             layout2.Add(avatar);
             layout2.Add(layout);

             TapGestureRecognizer tapRecognizer = new TapGestureRecognizer();
             tapRecognizer.Tapped += async (s, e) =>
             {
                 object bindingContext = (s as View).BindingContext;
                 await OpenChat(bindingContext);
             };

             layout2.GestureRecognizers.Add(tapRecognizer);

             return layout2;
         });
        chatList.ItemsSource = Chats;
        Loaded += async (s, e) =>
        {
            await Navigation.PushModalAsync(new ConnectionPage());
        };
    }

    public async Task OpenChat(object value)
    {
        ChatModel chat = (ChatModel)value;
        await Navigation.PushModalAsync(new ChatPage(chat));
    }

    public static ChatsPage Current
    {
        get; set;
    }

    public static ObservableCollection<ChatModel> Chats
    {
        get; set;
    } = new ObservableCollection<ChatModel>();
}