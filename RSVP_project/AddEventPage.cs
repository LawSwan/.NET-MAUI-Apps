namespace RSVPProject;

public sealed class AddEventPage : ContentPage
{
    readonly Entry title = LoginPage.Field("Event name", Keyboard.Default);
    readonly DatePicker date = new() { Date = DateTime.Today.AddDays(7), TextColor = Colors.Black, BackgroundColor = Color.FromArgb("#EDE7FB") };
    readonly Entry location = LoginPage.Field("Location", Keyboard.Default);
    readonly Editor description = new() { Placeholder = "Description", HeightRequest = 110, TextColor = Colors.Black, PlaceholderColor = Color.FromArgb("#8570D6"), BackgroundColor = Color.FromArgb("#EDE7FB") };
    readonly Label message = LoginPage.MessageLabel();

    public AddEventPage()
    {
        Title = "Add event";
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
