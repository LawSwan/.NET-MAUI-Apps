namespace AssessmentApp.Services;

public static class AppSettings
{
    public const string DisplayName = "Lawson";
    public const string LoginUserName = "Lawson01";
    public const string LoginPassword = "Password1";

    public static string ApiBaseUrl => DeviceInfo.Platform == DevicePlatform.Android
        ? "http://10.0.2.2:5024/"
        : "http://localhost:5024/";
}