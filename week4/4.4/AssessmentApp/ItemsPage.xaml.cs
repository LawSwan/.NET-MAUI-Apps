namespace AssessmentApp;

/// <summary>
/// Data entry page. All logic lives in <see cref="ViewModels.ItemsViewModel"/>.
/// </summary>
public partial class ItemsPage : ContentPage
{
	private readonly ViewModels.ItemsViewModel viewModel;

	/// <param name="viewModel">The page's view model, provided by dependency injection.</param>
	public ItemsPage(ViewModels.ItemsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = this.viewModel = viewModel;
	}

	/// <summary>
	/// Loads the stored items from the web service each time the page is shown.
	/// </summary>
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.LoadCommand.ExecuteAsync(null);
	}
}
