namespace VKanave.Views;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
		AddTapRecognizer(vkImg, "https://vk.com/hellocsharp");
        AddTapRecognizer(steamImg, "https://steamcommunity.com/id/HelloCSharp");
        //AddTapRecognizer(discordImg, "https://vk.com/hellocsharp/");
        AddTapRecognizer(githubImg, "https://github.com/blyatArtem/VKanave");
        AddTapRecognizer(textbox1, "https://github.com/blyatArtem/VKanave");
    }

	private void AddTapRecognizer(View img, string url)
	{
		var recognizer = new TapGestureRecognizer();
		recognizer.Tapped += (s, e) => OpenUrl(url);
        img.GestureRecognizers.Add(recognizer);
	}

	private void OpenUrl(string url)
	{
        Browser.Default.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
    }
}