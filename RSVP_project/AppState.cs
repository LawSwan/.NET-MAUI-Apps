namespace RSVPProject;

public static class AppState
{
    public static bool IsLoggedIn { get; private set; }
    public static bool IsGuest { get; private set; }
    public static string DisplayName { get; private set; } = string.Empty;

    public static void StartUserSession(string displayName)
    {
        IsLoggedIn = true;
        IsGuest = false;
        DisplayName = displayName;
    }

    public static void StartGuestSession()
    {
        IsLoggedIn = false;
        IsGuest = true;
        DisplayName = "Guest";
    }

    public static void LogOut()
    {
        IsLoggedIn = false;
        IsGuest = false;
        DisplayName = string.Empty;
    }
}

public sealed class EventItem
{
    public string Title { get; init; } = string.Empty;
    public string Date { get; init; } = string.Empty;
    public string Time { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public string Host { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool IsAttending { get; init; }
    public bool IsHosted { get; init; }
}

public static class SampleEvents
{
    public static IReadOnlyList<EventItem> All { get; } = new[]
    {
        new EventItem { Title = "Rooftop Film Night", Date = "Fri, Sep 18", Time = "7:30 PM", Location = "The Lantern Rooftop", Host = "Maya Chen", Description = "An open-air screening, warm blankets, and a small menu of late-summer snacks.", IsAttending = true },
        new EventItem { Title = "Sunday Pasta Club", Date = "Sun, Sep 20", Time = "5:00 PM", Location = "12 Olive Street", Host = "You", Description = "A relaxed evening of handmade pasta, shared plates, and good conversation.", IsHosted = true },
        new EventItem { Title = "City Garden Volunteer Day", Date = "Sat, Sep 26", Time = "10:00 AM", Location = "Northside Community Garden", Host = "Jordan Lee", Description = "Help refresh the garden beds and stay for coffee with the neighborhood crew." },
        new EventItem { Title = "Indie Makers Market", Date = "Sun, Sep 27", Time = "11:00 AM", Location = "Foundry Hall", Host = "Maya Chen", Description = "Local makers, prints, ceramics, and a corner for live acoustic sets." }
    };
}
