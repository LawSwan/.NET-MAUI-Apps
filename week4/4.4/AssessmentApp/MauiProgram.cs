using Microsoft.Extensions.Logging;
using AssessmentApp.Services;
using AssessmentApp.ViewModels;

namespace AssessmentApp;

public static class MauiProgram
{
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

		builder.Services.AddSingleton(new HttpClient
		{
			BaseAddress = new Uri(AppSettings.ApiBaseUrl)
		});
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
