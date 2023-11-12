using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using VKanave.Extensions;
using VKanave.Models;

namespace VKanave.Views;

public partial class ChatsPage : ContentPage
{
    public ChatsPage()
    {
        InitializeComponent();
        chatList.ItemTemplate = new DataTemplate(() =>
         {
             StackLayout layout = new StackLayout();
             StackLayout layout2 = new StackLayout();
             AvatarView avatar = new AvatarView();

             avatar.ImageSource = ImageSource.FromFile("avatar.png");
             avatar.Text = "null";
             avatar.BorderWidth = 0;

             layout.Padding = 5;
             layout.BackgroundColor = Color.FromHex($"#272938");
             layout.HorizontalOptions = LayoutOptions.FillAndExpand;
             layout.Background = new LinearGradientBrush(new GradientStopCollection()
             {
                 new GradientStop(Color.FromHex("#1A1A26"), 0.0f),
                 new GradientStop(Color.FromHex("#272938"), 1.0f)
             }, new Point(0, 0), new Point(1, 1));

             layout2.Orientation = StackOrientation.Horizontal;
             layout2.HorizontalOptions = LayoutOptions.FillAndExpand;
             layout2.Margin = new Thickness(0, 1);

             Label labelUsername = new Label { FontSize = 16, TextColor = Color.FromHex("#879EEC"), FontAttributes = FontAttributes.Bold };
             labelUsername.SetBinding(Label.TextProperty, new Binding("User.Username"));

             Label labelLastMessage = new Label { FontSize = 14, TextColor = Colors.Silver };
             labelLastMessage.SetBinding(Label.TextProperty, new Binding("LastMessage.Preview"));

             Label labelDate = new Label { FontSize = 10, TextColor = Colors.Gray };
             labelDate.SetBinding(Label.TextProperty, new Binding("LastMessage.DateTime"));
             labelDate.HorizontalOptions = LayoutOptions.EndAndExpand;

             layout.Add(labelUsername);
             layout.Add(labelLastMessage);
             layout.Add(labelDate);

             layout2.Add(avatar);
             layout2.Add(layout);

             return layout2;
         });
        chatList.ItemsSource = Chats;
        Loaded += async (s, e) =>
        {
            await Navigation.PushModalAsync(new ConnectionPage());
            //Chats.Add(new ChatModel(new UserModel("User#1915"), new MessageModel(0, "Message!", DateTime.UtcNow.ToUnixTime())));
            //Chats.Add(new ChatModel(new UserModel("User#1311"), new MessageModel(0, "Hello. Nigger pidoras asd dsa asd d sada sda ", DateTime.UtcNow.ToUnixTime())));
            //Chats.Add(new ChatModel(new UserModel("User#5618"), new MessageModel(0, "ahahha", DateTime.UtcNow.ToUnixTime())));
        };
    }

    public object LoadChatTemplate()
    {
        return null;
    }

    public static ObservableCollection<ChatModel> Chats
    {
        get; set;
    } = new ObservableCollection<ChatModel>();
}