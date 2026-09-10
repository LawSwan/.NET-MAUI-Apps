namespace RSVPProject;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnLoginClicked(object? sender, EventArgs e) => await Navigation.PushAsync(new LoginPage());
	private async void OnGuestClicked(object? sender, EventArgs e)
	{
		AppState.StartGuestSession();
		await Navigation.PushAsync(new EventsPage());
	}
	private async void OnCreateAccountClicked(object? sender, EventArgs e) => await Navigation.PushAsync(new AddUserPage());
}
