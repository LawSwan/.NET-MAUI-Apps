using Microsoft.Extensions.DependencyInjection;

namespace AssessmentApp;

/// <summary>
/// Application entry point: loads the shared styles (App.xaml) and creates the main window.
/// </summary>
public partial class App : Application
{
	private readonly IServiceProvider services;

	/// <param name="services">Dependency injection container built in MauiProgram.</param>
	public App(IServiceProvider services)
	{
		InitializeComponent();
		this.services = services;
	}

	/// <summary>
	/// Creates the window that hosts AppShell.
	/// </summary>
	protected override Window CreateWindow(IActivationState? activationState)
	{
		// Resolve the shell here (not in the constructor) so App.xaml resources are loaded first.
		return new Window(services.GetRequiredService<AppShell>());
	}
}
