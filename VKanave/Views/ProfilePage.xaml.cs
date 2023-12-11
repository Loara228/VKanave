using CommunityToolkit.Maui.Views;
using System.Security.Cryptography.X509Certificates;
using VKanave.Models;
using VKanave.Networking;
using VKanave.Networking.NetMessages;
using VKanave.Views.Popups;

namespace VKanave.Views;

public partial class ProfilePage : ContentPage
{
    /// <summary>
    /// Local
    /// </summary>
    public ProfilePage()
    {
        InitializeComponent();

        Local = true;
        Current = this;

        _profileInfo = new ProfileInfoModel();
        InitPage();
    }

    public ProfilePage(ProfileInfoModel profileInfoModel)
    {
        InitializeComponent();

        Local = false;
        Current = this;

        _profileInfo = profileInfoModel;
        InitPage();
        Networking.Networking.Send(new NMLoadProfile() { userId = profileInfoModel.userId });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ChatPage.navigateToProfile = false;
    }

    private void InitPage()
    {
        if (!Local)
        {
            imageEditor1.IsVisible = false;
            labelBioCaption.Text = "Bio";
        }

        this.Title = _profileInfo.Username + "'s profile";

        labelDisplayName.BindingContext = _profileInfo;
        labelUsername.BindingContext = _profileInfo;
        labelReg.BindingContext = _profileInfo;
        labelBio.BindingContext = _profileInfo;
        labelLastActive.BindingContext = _profileInfo;

        labelDisplayName.SetBinding(Label.TextProperty, new Binding("DisplayName"));
        labelUsername.SetBinding(Label.TextProperty, new Binding("Username"));
        labelReg.SetBinding(Label.TextProperty, new Binding("Reg"));
        labelBio.SetBinding(Label.TextProperty, new Binding("Bio"));
        labelLastActive.SetBinding(Label.TextProperty, new Binding("LastActive"));

        InitEvents();
    }

    private void InitEvents()
    {
        if (!Local)
            return;
        TapGestureRecognizer tapRecognizer = new TapGestureRecognizer();
        tapRecognizer.Tapped += (s, e) => EditBio();
        layoutBio.GestureRecognizers.Add(tapRecognizer);

        TapGestureRecognizer tapRecognizer2 = new TapGestureRecognizer();
        tapRecognizer2.Tapped += (s, e) => EditDisplayname();
        displayNameLayout.GestureRecognizers.Add(tapRecognizer2);
    }

    private void EditBio()
    {
        TextboxPopup tbEditor = new TextboxPopup("Bio", "set bio", new Action<string>((res) =>
        {
            _profileInfo.Bio = res;
            Networking.Networking.Send(new NMSetBio() { userId = LocalUser.Id, bio = res });
        }));
        this.ShowPopup(tbEditor);
    }

    private void EditDisplayname()
    {
        TextboxPopup tbEditor = new TextboxPopup("Display Name", "set display name", new Action<string>((res) =>
        {
            _profileInfo.DisplayName = res;
            Networking.Networking.Send(new NMSetDisplayname() { userId = LocalUser.Id, displayName = res });
        }));
        this.ShowPopup(tbEditor);
    }

    public bool Local
    {
        get; init;
    }

    public static ProfilePage Current;

    public ProfileInfoModel _profileInfo;
}