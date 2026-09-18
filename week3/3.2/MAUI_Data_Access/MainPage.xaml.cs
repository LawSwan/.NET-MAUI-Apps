using System.Collections.ObjectModel;
using MAUI_Data_Access.DataAccess;
using MAUI_Data_Access.Models;

namespace MAUI_Data_Access;

public partial class MainPage : ContentPage
{
	private readonly PersonData personData;

	public ObservableCollection<Person> People { get; } = new();

	public MainPage()
	{
		InitializeComponent();
		personData = new PersonData();
		BindingContext = this;
		_ = UpdatePeopleListAsync();
	}

	private async void OnSaveClicked(object? sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(txtFirstName.Text))
		{
			await DisplayAlertAsync("Error", "First name cannot be empty", "OK");
			return;
		}

		if (string.IsNullOrWhiteSpace(txtLastName.Text))
		{
			await DisplayAlertAsync("Error", "Last name cannot be empty", "OK");
			return;
		}

		if (dpDateOfBirth.Date is not DateTime dateOfBirth)
		{
			await DisplayAlertAsync("Error", "Date of Birth is required", "OK");
			return;
		}

		if (dateOfBirth > DateTime.Today)
		{
			await DisplayAlertAsync("Error", "Date of Birth cannot be in the future", "OK");
			return;
		}

		var person = new Person
		{
			FirstName = txtFirstName.Text.Trim(),
			LastName = txtLastName.Text.Trim(),
			DoB = dateOfBirth
		};

		await personData.SavePersonAsync(person);
		await UpdatePeopleListAsync();
		txtFirstName.Text = string.Empty;
		txtLastName.Text = string.Empty;
	}

	private async Task UpdatePeopleListAsync()
	{
		var people = await personData.GetPeopleAsync();
		People.Clear();

		foreach (var person in people)
		{
			People.Add(person);
		}
	}
}
