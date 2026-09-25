using AssessmentApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AssessmentApp.ViewModels;

public partial class LoginViewModel(ApiService apiService) : ObservableObject
{
    [ObservableProperty] private string username = string.Empty;
    [ObservableProperty] private string password = string.Empty;
    [ObservableProperty] private string message = string.Empty;
    [ObservableProperty] private bool isBusy;

    public string DisplayName => AppSettings.DisplayName;

    [RelayCommand]
    private async Task LoginAsync()
    {
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
                await Shell.Current.GoToAsync("items");
            }
            else
            {
                Message = "Login failed. Please check your username and password.";
            }
        }
        catch (HttpRequestException)
        {
            Message = "Could not connect to the web service.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        Username = string.Empty;
        Password = string.Empty;
        Message = string.Empty;
    }
}