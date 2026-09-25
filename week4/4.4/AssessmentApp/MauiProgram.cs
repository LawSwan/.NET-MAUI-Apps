using Microsoft.Extensions.Logging;
using AssessmentApp.Services;
using AssessmentApp.ViewModels;

namespace AssessmentApp;

/// <summary>
/// Configures the app: fonts, the HttpClient for the web service, and dependency injection
/// registrations for services, view models and pages.
/// </summary>
public static class MauiProgram
{
	/// <summary>
	/// Builds the MAUI app. Called by each platform's startup code.
	/// </summary>
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// One HttpClient shared by the whole app, pointed at the AssessmentApi web service.
		builder.Services.AddSingleton(new HttpClient
		{
			BaseAddress = new Uri(AppSettings.ApiBaseUrl)
		});

		// Services, view models and pages are created by dependency injection.
		builder.Services.AddSingleton<ApiService>();
		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<ItemsViewModel>();
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<ItemsPage>();
		builder.Services.AddTransient<AppShell>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
