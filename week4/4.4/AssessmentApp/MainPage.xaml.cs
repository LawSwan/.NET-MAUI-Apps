namespace AssessmentApp;

public partial class MainPage : ContentPage
{
	public MainPage(ViewModels.LoginViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
