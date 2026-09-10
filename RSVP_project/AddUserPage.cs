namespace RSVPProject;

public sealed class AddUserPage : ContentPage
{
    readonly Entry name = LoginPage.Field("Full name", Keyboard.Default);
    readonly Entry email = LoginPage.Field("Email", Keyboard.Email);
    readonly Entry password = LoginPage.Field("Password", Keyboard.Default, true);
    readonly Entry confirm = LoginPage.Field("Confirm password", Keyboard.Default, true);
    readonly Label message = LoginPage.MessageLabel();

    public AddUserPage()
    {
        Title = "Create account";
        BackgroundColor = Color.FromArgb("#101827");
        Content = new ScrollView
        {
            Content = LoginPage.FormLayout("Create your account", "A few details are all you need to start making plans.",
                name, email, password, confirm, message,
                LoginPage.Button("Add user", OnAdd),
                LoginPage.Button("Cancel", async (_, _) => await Navigation.PopAsync(), "SecondaryButton"))
        };
    }

    async void OnAdd(object? sender, EventArgs args)
    {
        if (new[] { name.Text, email.Text, password.Text, confirm.Text }.Any(string.IsNullOrWhiteSpace))
        {
            message.Text = "Please complete every field.";
            return;
        }
        if (!password.Text!.Equals(confirm.Text, StringComparison.Ordinal))
        {
            message.Text = "Passwords must match.";
            return;
        }

        await DisplayAlertAsync("User added", "Your account details are ready for the next step.", "OK");
        await Navigation.PopAsync();
    }
}
