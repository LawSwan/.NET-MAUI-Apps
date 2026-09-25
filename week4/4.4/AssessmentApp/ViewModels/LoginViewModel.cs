using AssessmentApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AssessmentApp.ViewModels;

/// <summary>
/// Data and commands for the login page (MainPage). The [ObservableProperty] fields generate
/// bindable properties (Username, Password, Message, IsBusy) and [RelayCommand] methods
/// generate the LoginCommand and CancelCommand the buttons bind to.
/// </summary>
/// <param name="apiService">Used to check the credentials with the web service.</param>
public partial class LoginViewModel(ApiService apiService) : ObservableObject
{
    /// <summary>Text in the "User Name" entry.</summary>
    [ObservableProperty] private string username = string.Empty;

    /// <summary>Text in the "Password" entry.</summary>
    [ObservableProperty] private string password = string.Empty;

    /// <summary>Error message shown under the buttons (empty when there is nothing to show).</summary>
    [ObservableProperty] private string message = string.Empty;

    /// <summary>True while waiting on the web service; shows the activity indicator.</summary>
    [ObservableProperty] private bool isBusy;

    /// <summary>Name shown at the top of the page.</summary>
    public string DisplayName => AppSettings.DisplayName;

    /// <summary>
    /// Login button: authenticates with the web service. On success opens the data entry
    /// page; otherwise shows a login failed message.
    /// </summary>
    [RelayCommand]
    private async Task LoginAsync()
    {
        // Ignore extra clicks while a login is already in progress.
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        Message = string.Empty;
        try
        {
            if (await apiService.AuthenticateAsync(Username, Password))
            {
                // "items" is the ItemsPage route registered in AppShell.
                await Shell.Current.GoToAsync("items");
            }
            else
            {
                Message = "Login failed. Please check your username and password.";
            }
        }
        catch (HttpRequestException)
        {
            Message = "Could not connect to the web service. Make sure AssessmentApi is running.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Cancel button: clears the user name, password and any message.
    /// </summary>
    [RelayCommand]
    private void Cancel()
    {
        Username = string.Empty;
        Password = string.Empty;
        Message = string.Empty;
    }
}
