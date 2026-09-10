namespace RSVPProject;

public sealed class RsvpPage : ContentPage
{
    readonly Entry name = LoginPage.Field("Your name", Keyboard.Default);
    readonly Entry email = LoginPage.Field("Email", Keyboard.Email);
    readonly Entry guests = LoginPage.Field("Number of guests", Keyboard.Numeric);
    readonly Editor notes = new() { Placeholder = "Notes for the host", HeightRequest = 100, TextColor = Colors.White, PlaceholderColor = Color.FromArgb("#718098"), BackgroundColor = Color.FromArgb("#172337") };
    readonly Label message = LoginPage.MessageLabel();

    public RsvpPage(EventItem item)
    {
        Title = "RSVP";
        BackgroundColor = Color.FromArgb("#101827");
        if (AppState.IsLoggedIn)
        {
            name.Text = AppState.DisplayName;
            email.Text = "demo@rsvp.app";
            guests.Text = "1";
        }

        Content = new ScrollView { Content = LoginPage.FormLayout("RSVP for this event", item.Title, name, email, guests, notes, message,
            LoginPage.Button("Save RSVP", OnSave), LoginPage.Button("Cancel", async (_, _) => await Navigation.PopAsync(), "SecondaryButton")) };
    }

    async void OnSave(object? sender, EventArgs args)
    {
        if (new[] { name.Text, email.Text, guests.Text, notes.Text }.Any(string.IsNullOrWhiteSpace))
        {
            message.Text = "Please complete every field.";
            return;
        }
        await DisplayAlertAsync("RSVP noted", "RSVP functionality will be connected in a later week.", "OK");
        await Navigation.PopAsync();
    }
}
