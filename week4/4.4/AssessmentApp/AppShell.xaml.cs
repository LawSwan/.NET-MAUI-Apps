namespace AssessmentApp;

public partial class AppShell : Shell
{
	public AppShell(MainPage loginPage)
	{
		InitializeComponent();
		Items.Add(new ShellContent
		{
			Title = "Login",
			Route = "MainPage",
			Content = loginPage
		});
		Routing.RegisterRoute("items", typeof(ItemsPage));
	}
}
