using System.Net.Http.Json;
using MAUI_Data_Access.Models;

namespace MAUI_Data_Access.DataAccess;

public class PersonData
{
	private static readonly HttpClient Http = new()
	{
		BaseAddress = new Uri("http://localhost:5289")
	};

	public async Task<List<Person>> GetPeopleAsync()
	{
		return await Http.GetFromJsonAsync<List<Person>>("api/Person") ?? new List<Person>();
	}

	public async Task<Person?> GetPersonAsync(int id)
	{
		return await Http.GetFromJsonAsync<Person>($"api/Person/{id}");
	}

	public async Task<int> SavePersonAsync(Person person)
	{
		var response = await Http.PostAsJsonAsync("api/Person", person);
		response.EnsureSuccessStatusCode();
		return 1;
	}

	public async Task<int> DeletePersonAsync(Person person)
	{
		var response = await Http.DeleteAsync($"api/Person/{person.ID}");
		response.EnsureSuccessStatusCode();
		return 1;
	}
}