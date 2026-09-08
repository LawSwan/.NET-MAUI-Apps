namespace MauiPageNavigation;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnSendMessageClicked(object? sender, EventArgs e)
	{
		await Shell.Current.Navigation.PushAsync(new MessagePage(txtMessage.Text));
	}
}
