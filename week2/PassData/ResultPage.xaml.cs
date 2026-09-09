namespace PassData;

public enum ResultOperation
{
	Addition,
	Subtraction
}

public partial class ResultPage : ContentPage
{
	public ResultPage(
		string firstValue,
		string secondValue,
		double firstNumber,
		double secondNumber,
		ResultOperation operation)
	{
		InitializeComponent();

		NumbersEnteredLabel.Text = $"The numbers entered are {firstValue} and {secondValue}.";
		ResultLabel.Text = operation == ResultOperation.Addition
			? $"The sum of the numbers is {firstNumber + secondNumber}"
			: $"The difference of the numbers is {firstNumber - secondNumber}";
	}

	private async void OnGoBackClicked(object? sender, EventArgs e)
	{
		await Shell.Current.Navigation.PopAsync();
	}
}