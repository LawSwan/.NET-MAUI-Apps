namespace RSVPProject;

public sealed class LoginPage : ContentPage
{
    readonly Entry email = Field("Email", Keyboard.Email);
    readonly Entry password = Field("Password", Keyboard.Default, true);
    readonly Label message = MessageLabel();

    const string DemoHint = "Demo credentials: demo@rsvp.app / rsvp123";

    public LoginPage()
    {
        Title = "Log in";
        ShowHint(DemoHint);
        Content = new ScrollView { Content = FormLayout("Welcome back", "Use the demo account to explore the logged-in experience.",
            email, password, message,
            Button("Log in", OnLogin),
            Button("Create an account", async (_, _) => await Navigation.PushAsync(new AddUserPage()), "TextButton"),
            Button("Continue as guest", OnGuest, "SecondaryButton")) };
    }

    async void OnLogin(object? sender, EventArgs args)
    {
        if (string.IsNullOrWhiteSpace(email.Text) || string.IsNullOrWhiteSpace(password.Text))
        {
            ShowError("Enter your email and password to continue.");
            return;
        }

        if (email.Text.Trim().Equals("demo@rsvp.app", StringComparison.OrdinalIgnoreCase) && password.Text == "rsvp123")
        {
            AppState.StartUserSession("Amber Lawson");
            await Navigation.PushAsync(new EventsPage());
            return;
        }

        ShowError(DemoHint);
    }

    // Shown up front, in the muted purple used for secondary text — not an error yet.
    void ShowHint(string text)
    {
        message.Text = text;
        message.TextColor = Color.FromArgb("#8570D6");
    }

    // Shown after a failed attempt, in the error color.
    void ShowError(string text)
    {
        message.Text = text;
        message.TextColor = Color.FromArgb("#F27F6B");
    }

    async void OnGuest(object? sender, EventArgs args)
    {
        AppState.StartGuestSession();
        await Navigation.PushAsync(new EventsPage());
    }

    internal static Entry Field(string placeholder, Keyboard keyboard, bool secret = false) => new()
    {
        Placeholder = placeholder,
        Keyboard = keyboard,
        IsPassword = secret,
        BackgroundColor = Color.FromArgb("#EDE7FB"),
        TextColor = Colors.Black,
        PlaceholderColor = Color.FromArgb("#8570D6"),
        Margin = new Thickness(0, 4)
    };

    internal static Label MessageLabel() => new() { TextColor = Color.FromArgb("#F27F6B"), FontSize = 13 };

    internal static Button Button(string text, EventHandler handler, string? style = null)
    {
        var button = new Button { Text = text };
        if (style is not null) button.Style = (Style)Application.Current!.Resources[style];
        button.Clicked += handler;
        return button;
    }

    /// <summary>
    /// Shared form shell used by every account/RSVP form: a purple-gradient
    /// header (heading + subtitle) over a plain white body holding the fields.
    /// </summary>
    internal static View FormLayout(string heading, string subtitle, params View[] controls)
    {
        var header = new Border
        {
            Background = (Brush)Application.Current!.Resources["PrimaryGradientBrush"],
            Stroke = Colors.Transparent,
            Padding = new Thickness(24, 56, 24, 28)
        };
        header.Content = new VerticalStackLayout
        {
            Spacing = 6,
            Children =
            {
                new Label { Text = heading, TextColor = Colors.White, FontSize = 28, FontAttributes = FontAttributes.Bold },
                new Label { Text = subtitle, TextColor = Colors.White, Opacity = 0.85, FontSize = 15 }
            }
        };

        var body = new VerticalStackLayout { Padding = new Thickness(24, 24), Spacing = 12 };
        foreach (var control in controls) body.Add(control);

        return new VerticalStackLayout { Children = { header, body } };
    }
}
