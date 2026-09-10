namespace RSVPProject;

public sealed class LoginPage : ContentPage
{
    readonly Entry email = Field("Email", Keyboard.Email);
    readonly Entry password = Field("Password", Keyboard.Default, true);
    readonly Label message = MessageLabel();

    public LoginPage()
    {
        Title = "Log in";
        BackgroundColor = Colors.Transparent;
        Content = WithBackground(FormLayout("Welcome back", "Use the demo account to explore the logged-in experience.",
            email, password, message,
            Button("Log in", OnLogin),
            Button("Create an account", async (_, _) => await Navigation.PushAsync(new AddUserPage()), "TextButton"),
            Button("Continue as guest", OnGuest, "SecondaryButton")));
    }

    async void OnLogin(object? sender, EventArgs args)
    {
        if (string.IsNullOrWhiteSpace(email.Text) || string.IsNullOrWhiteSpace(password.Text))
        {
            message.Text = "Enter your email and password to continue.";
            return;
        }

        if (email.Text.Trim().Equals("demo@rsvp.app", StringComparison.OrdinalIgnoreCase) && password.Text == "rsvp123")
        {
            AppState.StartUserSession("Amber Lawson");
            await Navigation.PushAsync(new EventsPage());
            return;
        }

        message.Text = "Demo credentials: demo@rsvp.app / rsvp123";
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
        BackgroundColor = Color.FromArgb("#172337"),
        TextColor = Colors.White,
        PlaceholderColor = Color.FromArgb("#f7f8f8"),
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

    internal static VerticalStackLayout FormLayout(string heading, string subtitle, params View[] controls)
    {
        var layout = new VerticalStackLayout { Padding = new Thickness(24, 30), Spacing = 12 };
        layout.Add(new Label { Text = heading, TextColor = Colors.White, FontSize = 30, FontAttributes = FontAttributes.Bold });
        layout.Add(new Label { Text = subtitle, TextColor = Color.FromArgb("#AAB6C7"), FontSize = 15, Margin = new Thickness(0, 0, 0, 12) });
        foreach (var control in controls) layout.Add(control);
        return layout;
    }

    internal static View WithBackground(View content)
    {
        var background = new Image
        {
            Source = "rsvp_background.svg",
            Aspect = Aspect.AspectFill
        };
        return new Grid { Children = { background, new ScrollView { Content = content } } };
    }
}
