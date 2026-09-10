namespace RSVPProject;

public sealed class AddEventPage : ContentPage
{
    readonly Entry title = LoginPage.Field("Event name", Keyboard.Default);
    readonly DatePicker date = new() { Date = DateTime.Today.AddDays(7), TextColor = Colors.White, BackgroundColor = Color.FromArgb("#172337") };
    readonly Entry location = LoginPage.Field("Location", Keyboard.Default);
    readonly Editor description = new() { Placeholder = "Description", HeightRequest = 110, TextColor = Colors.White, PlaceholderColor = Color.FromArgb("#718098"), BackgroundColor = Color.FromArgb("#172337") };
    readonly Label message = LoginPage.MessageLabel();

    public AddEventPage()
    {
        Title = "Add event";
        BackgroundColor = Color.FromArgb("#101827");
        Content = new ScrollView { Content = LoginPage.FormLayout("Add an event", "Set the scene now. Saving will be connected later.", title, date, location, description, message,
            LoginPage.Button("Save event", OnSave), LoginPage.Button("Cancel", async (_, _) => await Navigation.PopAsync(), "SecondaryButton")) };
    }

    async void OnSave(object? sender, EventArgs args)
    {
        if (string.IsNullOrWhiteSpace(title.Text) || string.IsNullOrWhiteSpace(location.Text) || string.IsNullOrWhiteSpace(description.Text))
        {
            message.Text = "Please complete every field.";
            return;
        }
        await DisplayAlertAsync("Event ready", "Event saving will be connected in a later week.", "OK");
        await Navigation.PopAsync();
    }
}
