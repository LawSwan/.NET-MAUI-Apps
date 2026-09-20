using RSVPProject.Data;

namespace RSVPProject;

public sealed class EventDetailsPage : ContentPage
{
    readonly EventRecord eventRecord;

    public EventDetailsPage(EventRecord item)
    {
        eventRecord = item;
        Title = item.Title;

        var header = new Border
        {
            Background = (Brush)Application.Current!.Resources["PrimaryGradientBrush"],
            Stroke = Colors.Transparent,
            Padding = new Thickness(24, 32, 24, 24)
        };
        header.Content = new VerticalStackLayout
        {
            Spacing = 6,
            Children =
            {
                new Label { Text = item.Title, TextColor = Colors.White, FontSize = 28, FontAttributes = FontAttributes.Bold },
                new Label { Text = $"{item.DateDisplay}  •  {item.TimeDisplay}", TextColor = Colors.White, Opacity = 0.9, FontSize = 15 },
                new Label { Text = item.Location, TextColor = Colors.White, Opacity = 0.75, FontSize = 14 }
            }
        };

        var body = new VerticalStackLayout { Padding = new Thickness(24, 20), Spacing = 14 };
        body.Add(new BoxView { HeightRequest = 1 });
        body.Add(new Label { Text = item.Description, TextColor = Colors.Black, FontSize = 17, LineBreakMode = LineBreakMode.WordWrap });
        body.Add(new Label { Text = $"Hosted by {item.HostName}", TextColor = Color.FromArgb("#8570D6"), FontSize = 14 });
        body.Add(LoginPage.Button("RSVP for this event", async (_, _) => await Navigation.PushAsync(new RsvpPage(item))));
        body.Add(LoginPage.Button("Back to events", async (_, _) => await Navigation.PopAsync(), "SecondaryButton"));

        Content = new ScrollView { Content = new VerticalStackLayout { Children = { header, body } } };
    }
}
