using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using VKanave.Networking;
using VKanave.Networking.NetMessages;

namespace VKanave.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        Current = this;
        textboxUsername.Focused += (s, e) =>
        {
            frameUsername.BorderColor = Color.FromRgba(0, 0, 0, 0);
            textboxUsername.TextColor = Color.FromRgb(0, 0, 0);
            button1.IsEnabled = true;
        };
        textboxPassword.Focused += (s, e) =>
        {
            framePassword.BorderColor = Color.FromRgba(0, 0, 0, 0);
            textboxPassword.TextColor = Color.FromRgb(0, 0, 0);
            button1.IsEnabled = true;
        };
    }

    private void Button_SignIn_Clicked(object sender, EventArgs e)
    {
        if (!CheckInputs1())
        {
            Toast.Make("Username too long or too short.", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
            return;
        }
        if (!CheckInputs2())
        {
            Toast.Make("Password too long or too short.", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
            return;
        }
        button1.IsEnabled = false;
        textboxUsername.IsReadOnly = true;
        textboxPassword.IsReadOnly = true;
        if (AuthorizationMode)
            Networking.Networking.SendData(new NMAuth(textboxUsername.Text, textboxPassword.Text));
        else
            Networking.Networking.SendData(new NMReg(textboxUsername.Text, textboxPassword.Text));
    }

    private bool CheckInputs1()
    {
        if (string.IsNullOrEmpty(textboxUsername.Text))
            return false;
        if (textboxUsername.Text.Length <= 3 || textboxUsername.Text.Length > 20)
            return false;
        return true;
    }

    private bool CheckInputs2()
    {
        if (string.IsNullOrEmpty(textboxPassword.Text))
            return false;
        if (textboxPassword.Text.Length <= 3 || textboxPassword.Text.Length > 20)
            return false;
        return true;
    }

    public void SignIn(string token)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            textboxUsername.IsReadOnly = false;
            textboxPassword.IsReadOnly = false;
            if (string.IsNullOrEmpty(token))
            {
                MarkUsername();
                MarkPassword();
                Toast.Make("Incorect username or password.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
            }
            else
            {
                // auth successfully :)
                MauiProgram.Token = token;
                Navigation.PopModalAsync();
            }
        });
    }

    private void MarkUsername()
    {
        frameUsername.BorderColor = Color.FromRgb(255, 0, 0);
        textboxUsername.TextColor = Color.FromRgb(255, 0, 0);
    }

    private void MarkPassword()
    {
        framePassword.BorderColor = Color.FromRgb(255, 0, 0);
        textboxPassword.TextColor = Color.FromRgb(255, 0, 0);
    }

    public void SignUp(int code)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            textboxUsername.IsReadOnly = false;
            textboxPassword.IsReadOnly = false;
            button1.IsEnabled = true;
            if (code == 0)
                RegEvent_UnknownError();
            else if (code == 1)
                RegEvent_Successfully();
            else if (code == 2)
                RegEvent_AlreadyExists();
        });
    }

    private void RegEvent_UnknownError()
    {
        Toast.Make("Unknown error. Try again later.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
    }

    private void RegEvent_Successfully()
    {
        Toast.Make("Thank you for registrating.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
        if (!AuthorizationMode)
            TapGestureRecognizer_Tapped(null, null);
    }

    public void RegEvent_AlreadyExists()
    {
        MarkUsername();
        Toast.Make("A user with that username already exists.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (!_animation)
        {
            _animation = true;
            AuthorizationMode = !AuthorizationMode;
            if (AuthorizationMode)
            {
                var t1 = label1.RotateXTo(90, 200, Easing.Linear);
                var t2 = label2.TranslateTo(label2.TranslationX, label2.TranslationY - 50, 200, Easing.Linear);
                await Task.WhenAll(t1, t2);
                label1.Text = "Authorization";
                await label1.RotateXTo(0, 200, Easing.Linear);
            }
            else
            {
                var t1 = label1.RotateXTo(90, 200, Easing.Linear);
                var t2 = label2.TranslateTo(label2.TranslationX, label2.TranslationY + 50, 200, Easing.Linear);
                await Task.WhenAll(t1, t2);
                label1.Text = "Registration";
                await label1.RotateXTo(0, 200, Easing.Linear);
            }
            _animation = false;
        }
    }

    public static LoginPage Current;

    public bool AuthorizationMode
    {
        get => _auth;
        set
        {
            _auth = value;
            if (value)
            {
                //label1.Text = "Authorization";
                //label2.IsVisible = true;
                label3.Text = "Sign up";
                button1.Text = "Sign in";
            }
            else
            {
                //label1.Text = "Registration";
                //label2.IsVisible = false;
                label3.Text = "Sign in";
                button1.Text = "Sign up";
            }
        }
    }

    private bool _auth = true;
    private bool _animation = false;
}