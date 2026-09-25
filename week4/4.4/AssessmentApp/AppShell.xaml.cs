namespace AssessmentApp;

/// <summary>
/// App navigation. The login page is the only top-level page; the data entry page is a
/// registered route that is only opened after a successful login.
/// </summary>
public partial class AppShell : Shell
{
	/// <param name="loginPage">The login page, created by dependency injection.</param>
	public AppShell(MainPage loginPage)
	{
		InitializeComponent();
		Items.Add(new ShellContent
		{
			Title = "Login",
			Route = "MainPage",
			Content = loginPage
		});

		// LoginViewModel navigates here with Shell.Current.GoToAsync("items").
		Routing.RegisterRoute("items", typeof(ItemsPage));
	}
}
