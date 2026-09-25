namespace AssessmentApp;

/// <summary>
/// Login page. All logic lives in <see cref="ViewModels.LoginViewModel"/>.
/// </summary>
public partial class MainPage : ContentPage
{
	/// <param name="viewModel">The page's view model, provided by dependency injection.</param>
	public MainPage(ViewModels.LoginViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
