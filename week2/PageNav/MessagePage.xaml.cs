namespace MauiPageNavigation;

public partial class MessagePage : ContentPage
{
	public MessagePage(string message)
	{
		InitializeComponent();
		lblMessage.Text = message;
	}

	private async void OnGoBackClicked(object? sender, EventArgs e)
	{
		await Shell.Current.Navigation.PopAsync();
	}
}