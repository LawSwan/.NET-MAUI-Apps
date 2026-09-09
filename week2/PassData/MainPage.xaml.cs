namespace PassData;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnAdditionClicked(object? sender, EventArgs e)
	{
		await OpenResultPageAsync(ResultOperation.Addition);
	}

	private async void OnSubtractionClicked(object? sender, EventArgs e)
	{
		await OpenResultPageAsync(ResultOperation.Subtraction);
	}

	private async Task OpenResultPageAsync(ResultOperation operation)
	{
		string? firstValue = FirstValueEntry.Text?.Trim();
		string? secondValue = SecondValueEntry.Text?.Trim();

		if (string.IsNullOrWhiteSpace(firstValue) || string.IsNullOrWhiteSpace(secondValue))
		{
			StatusLabel.Text = operation == ResultOperation.Addition
				? "Please enter values for both fields to get the sum"
				: "Please enter values for both fields to get the difference";
			return;
		}

		if (!double.TryParse(firstValue, out double firstNumber) ||
			!double.TryParse(secondValue, out double secondNumber))
		{
			StatusLabel.Text = "Please enter valid numbers in both fields";
			return;
		}

		StatusLabel.Text = string.Empty;
		await Shell.Current.Navigation.PushAsync(
			new ResultPage(firstValue, secondValue, firstNumber, secondNumber, operation));
	}
}
