namespace AssessmentApp.Services;

/// <summary>
/// App-wide settings shared by the pages and services.
/// </summary>
public static class AppSettings
{
    /// <summary>Name shown at the top of every page.</summary>
    public const string DisplayName = "Lawson";

    /// <summary>
    /// Base address of the AssessmentApi web service (see its launchSettings.json, port 5024).
    /// The Android emulator reaches the host computer's localhost through 10.0.2.2.
    /// </summary>
    public static string ApiBaseUrl => DeviceInfo.Platform == DevicePlatform.Android
        ? "http://10.0.2.2:5024/"
        : "http://localhost:5024/";
}
