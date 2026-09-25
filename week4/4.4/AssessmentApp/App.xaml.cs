using Microsoft.Extensions.DependencyInjection;

namespace AssessmentApp;

public partial class App : Application
{
	private readonly IServiceProvider services;

	public App(IServiceProvider services)
	{
		InitializeComponent();
		this.services = services;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		// Resolve the shell here (not in the constructor) so App.xaml resources are loaded first.
		return new Window(services.GetRequiredService<AppShell>());
	}
}
