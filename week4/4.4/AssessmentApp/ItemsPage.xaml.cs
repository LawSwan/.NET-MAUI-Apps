namespace AssessmentApp;

public partial class ItemsPage : ContentPage
{
	private readonly ViewModels.ItemsViewModel viewModel;

	public ItemsPage(ViewModels.ItemsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = this.viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.LoadCommand.ExecuteAsync(null);
	}
}