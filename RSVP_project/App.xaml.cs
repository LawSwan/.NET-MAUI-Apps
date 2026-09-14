namespace RSVPProject;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// The app is designed for a single clean white + purple look; pin the
		// theme so the styles (nav bar, pickers, etc.) always render that way
		// regardless of the device's system theme.
		UserAppTheme = AppTheme.Light;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new NavigationPage(new MainPage()));
	}
}