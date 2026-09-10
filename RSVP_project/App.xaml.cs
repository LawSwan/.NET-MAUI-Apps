namespace RSVPProject;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// The app is designed for a single dark palette; pin it so the themed
		// styles (nav bar, pickers, etc.) don't fall back to the light variants.
		UserAppTheme = AppTheme.Dark;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new NavigationPage(new MainPage()));
	}
}