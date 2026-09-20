namespace RSVPProject;

/// <summary>The signed-in session for this run of the app (not persisted).</summary>
public static class AppState
{
    public static bool IsLoggedIn { get; private set; }
    public static bool IsGuest { get; private set; }
    public static int? CurrentUserId { get; private set; }
    public static string DisplayName { get; private set; } = string.Empty;
    public static string Email { get; private set; } = string.Empty;

    public static void StartUserSession(int userId, string displayName, string email)
    {
        IsLoggedIn = true;
        IsGuest = false;
        CurrentUserId = userId;
        DisplayName = displayName;
        Email = email;
    }

    public static void StartGuestSession()
    {
        IsLoggedIn = false;
        IsGuest = true;
        CurrentUserId = null;
        DisplayName = "Guest";
        Email = string.Empty;
    }

    public static void LogOut()
    {
        IsLoggedIn = false;
        IsGuest = false;
        CurrentUserId = null;
        DisplayName = string.Empty;
        Email = string.Empty;
    }
}
