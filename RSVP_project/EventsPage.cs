using Microsoft.Maui.Controls.Shapes;

namespace RSVPProject;

public sealed class EventsPage : ContentPage
{
    readonly CollectionView events = new();
    readonly Picker filter = new() { Title = "Show events" };

    public EventsPage()
    {
        Title = "Events";
        BackgroundColor = Colors.Transparent;
        filter.ItemsSource = AppState.IsLoggedIn ? new[] { "All events", "I'm attending", "I'm hosting" } : new[] { "All events" };
        filter.SelectedIndex = 0;
        filter.SelectedIndexChanged += (_, _) => RefreshEvents();

        events.SelectionMode = SelectionMode.Single;
        events.SelectionChanged += OnEventSelected;
        events.ItemTemplate = new DataTemplate(() =>
        {
            var title = new Label { TextColor = Colors.White, FontSize = 18, FontAttributes = FontAttributes.Bold };
            title.SetBinding(Label.TextProperty, nameof(EventItem.Title));
            var meta = new Label { TextColor = Color.FromArgb("#F2B84B"), FontSize = 13 };
            meta.SetBinding(Label.TextProperty, new Binding(nameof(EventItem.Date), stringFormat: "{0}  •  ") { });
            var location = new Label { TextColor = Color.FromArgb("#AAB6C7"), FontSize = 14 };
            location.SetBinding(Label.TextProperty, nameof(EventItem.Location));
            var card = new Border { Stroke = Color.FromArgb("#29384D"), BackgroundColor = Color.FromArgb("#172337"), StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) }, Padding = 16, Margin = new Thickness(0, 0, 0, 10) };
            card.Content = new VerticalStackLayout { Spacing = 5, Children = { title, meta, location } };
            return card;
        });

        var footer = Footer();
        Content = LoginPage.WithBackground(new Grid { BackgroundColor = Colors.Transparent, RowDefinitions = new RowDefinitionCollection { new(GridLength.Auto), new(GridLength.Auto), new(GridLength.Star), new(GridLength.Auto) }, Children =
        {
            Header(),
            filter,
            events,
            footer
        }});
        Grid.SetRow(filter, 1); Grid.SetRow(events, 2); Grid.SetRow(footer, 3);
        RefreshEvents();
    }

    View Header() => new VerticalStackLayout { Padding = new Thickness(24, 28, 24, 10), Spacing = 4, Children =
    {
        new Label { Text = AppState.IsGuest ? "Browse events" : $"Hi, {AppState.DisplayName}", TextColor = Colors.White, FontSize = 28, FontAttributes = FontAttributes.Bold },
        new Label { Text = "Find your next reason to show up.", TextColor = Color.FromArgb("#AAB6C7") }
    }};

    View Footer() => new HorizontalStackLayout { Padding = new Thickness(24, 8), Spacing = 10, Children =
    {
        LoginPage.Button("Add event", async (_, _) => await Navigation.PushAsync(new AddEventPage())),
        LoginPage.Button("Log out", OnLogout, "SecondaryButton")
    }};

    void RefreshEvents()
    {
        events.ItemsSource = filter.SelectedIndex switch
        {
            1 => SampleEvents.All.Where(item => item.IsAttending).ToList(),
            2 => SampleEvents.All.Where(item => item.IsHosted).ToList(),
            _ => SampleEvents.All.ToList()
        };
    }

    async void OnEventSelected(object? sender, SelectionChangedEventArgs args)
    {
        if (args.CurrentSelection.FirstOrDefault() is EventItem selected)
        {
            events.SelectedItem = null;
            await Navigation.PushAsync(new EventDetailsPage(selected));
        }
    }

    async void OnLogout(object? sender, EventArgs args)
    {
        AppState.LogOut();
        await Navigation.PopToRootAsync();
    }
}
