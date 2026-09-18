using Microsoft.Extensions.DependencyInjection;

namespace ItemCatalog;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var appShell = Handler?.MauiContext?.Services.GetRequiredService<AppShell>()
			?? throw new InvalidOperationException("The application shell is not registered.");

		return new Window(appShell);
	}
}