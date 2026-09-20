using RSVPProject.Data;

namespace RSVPProject;

public sealed class AddEventPage : ContentPage
{
    readonly Entry title = LoginPage.Field("Event name", Keyboard.Default);
    readonly DatePicker date = new() { Date = DateTime.Today.AddDays(7), TextColor = Colors.Black, BackgroundColor = Color.FromArgb("#EDE7FB") };
    readonly TimePicker time = new() { Time = new TimeSpan(18, 0, 0), TextColor = Colors.Black, BackgroundColor = Color.FromArgb("#EDE7FB") };
    readonly Entry location = LoginPage.Field("Location", Keyboard.Default);
    readonly Editor description = new() { Placeholder = "Description", HeightRequest = 110, TextColor = Colors.Black, PlaceholderColor = Color.FromArgb("#8570D6"), BackgroundColor = Color.FromArgb("#EDE7FB") };
    readonly Label message = LoginPage.MessageLabel();

    public AddEventPage()
    {
        Title = "Add event";
        Content = new ScrollView { Content = LoginPage.FormLayout("Add an event", "Set the scene — it'll show up in the events list right away.", title, date, time, location, description, message,
            LoginPage.Button("Save event", OnSave), LoginPage.Button("Cancel", async (_, _) => await Navigation.PopAsync(), "SecondaryButton")) };
    }

    async void OnSave(object? sender, EventArgs args)
    {
        if (string.IsNullOrWhiteSpace(title.Text) || string.IsNullOrWhiteSpace(location.Text) || string.IsNullOrWhiteSpace(description.Text))
        {
            message.Text = "Please complete every field.";
            return;
        }

        var db = await AppDatabase.GetAsync();
        await db.AddEventAsync(new EventRecord
        {
            Title = title.Text!.Trim(),
            StartsAt = (date.Date?.Date ?? DateTime.Today) + (time.Time ?? TimeSpan.Zero),
            Location = location.Text!.Trim(),
            Description = description.Text!.Trim(),
            HostUserId = AppState.CurrentUserId,
            HostName = AppState.IsLoggedIn ? AppState.DisplayName : "Guest"
        });

        await DisplayAlertAsync("Event added", $"\"{title.Text.Trim()}\" is on the events list.", "OK");
        await Navigation.PopAsync();
    }
}
