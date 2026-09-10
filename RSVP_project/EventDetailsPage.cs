namespace RSVPProject;

public sealed class EventDetailsPage : ContentPage
{
    readonly EventItem eventItem;

    public EventDetailsPage(EventItem item)
    {
        eventItem = item;
        Title = item.Title;
        BackgroundColor = Colors.Transparent;
        var details = new VerticalStackLayout { Padding = new Thickness(24, 28), Spacing = 14 };
        details.Add(new Label { Text = item.Title, TextColor = Colors.White, FontSize = 30, FontAttributes = FontAttributes.Bold });
        details.Add(new Label { Text = $"{item.Date}  •  {item.Time}", TextColor = Color.FromArgb("#F2B84B"), FontSize = 16 });
        details.Add(new Label { Text = item.Location, TextColor = Color.FromArgb("#AAB6C7"), FontSize = 15 });
        details.Add(new BoxView { HeightRequest = 1, Color = Color.FromArgb("#29384D"), Margin = new Thickness(0, 8) });
        details.Add(new Label { Text = item.Description, TextColor = Colors.White, FontSize = 17, LineBreakMode = LineBreakMode.WordWrap });
        details.Add(new Label { Text = $"Hosted by {item.Host}", TextColor = Color.FromArgb("#AAB6C7"), FontSize = 14 });
        details.Add(LoginPage.Button("RSVP for this event", async (_, _) => await Navigation.PushAsync(new RsvpPage(item))));
        details.Add(LoginPage.Button("Back to events", async (_, _) => await Navigation.PopAsync(), "SecondaryButton"));
        Content = LoginPage.WithBackground(new ScrollView { Content = details });
    }
}
