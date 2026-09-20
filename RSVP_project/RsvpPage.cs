using RSVPProject.Data;

namespace RSVPProject;

public sealed class RsvpPage : ContentPage
{
    readonly EventRecord eventRecord;
    readonly Entry name = LoginPage.Field("Your name", Keyboard.Default);
    readonly Entry email = LoginPage.Field("Email", Keyboard.Email);
    readonly Entry guests = LoginPage.Field("Number of guests", Keyboard.Numeric);
    readonly Editor notes = new() { Placeholder = "Notes for the host (optional)", HeightRequest = 100, TextColor = Colors.Black, PlaceholderColor = Color.FromArgb("#8570D6"), BackgroundColor = Color.FromArgb("#EDE7FB") };
    readonly Label message = LoginPage.MessageLabel();

    public RsvpPage(EventRecord item)
    {
        eventRecord = item;
        Title = "RSVP";

        // If logged in, prepopulate with the signed-in user's info.
        if (AppState.IsLoggedIn)
        {
            name.Text = AppState.DisplayName;
            email.Text = AppState.Email;
            guests.Text = "1";
        }

        Content = new ScrollView { Content = LoginPage.FormLayout("RSVP for this event", item.Title, name, email, guests, notes, message,
            LoginPage.Button("Save RSVP", OnSave), LoginPage.Button("Cancel", async (_, _) => await Navigation.PopAsync(), "SecondaryButton")) };
    }

    async void OnSave(object? sender, EventArgs args)
    {
        if (string.IsNullOrWhiteSpace(name.Text) || string.IsNullOrWhiteSpace(email.Text) || string.IsNullOrWhiteSpace(guests.Text))
        {
            message.Text = "Please complete every field.";
            return;
        }
        if (!int.TryParse(guests.Text, out var guestCount) || guestCount < 1)
        {
            message.Text = "Enter a valid number of guests.";
            return;
        }

        var db = await AppDatabase.GetAsync();
        await db.AddRsvpAsync(new RsvpRecord
        {
            EventId = eventRecord.Id,
            UserId = AppState.CurrentUserId,
            GuestName = name.Text!.Trim(),
            GuestEmail = email.Text!.Trim(),
            GuestCount = guestCount,
            Notes = string.IsNullOrWhiteSpace(notes.Text) ? null : notes.Text.Trim()
        });

        await DisplayAlertAsync("RSVP saved", "You're on the list for this event.", "OK");
        await Navigation.PopAsync();
    }
}
