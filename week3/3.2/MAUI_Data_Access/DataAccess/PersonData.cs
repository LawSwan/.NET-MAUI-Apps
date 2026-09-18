using MAUI_Data_Access.Models;
using SQLite;

namespace MAUI_Data_Access.DataAccess;

public class PersonData
{
	private SQLiteAsyncConnection? database;

	private async Task Init()
	{
		if (database is not null)
		{
			return;
		}

		database = new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
		await database.CreateTableAsync<Person>();
	}

	public async Task<List<Person>> GetPeopleAsync()
	{
		await Init();
		return await database!.Table<Person>().ToListAsync();
	}

	public async Task<Person?> GetPersonAsync(int id)
	{
		await Init();
		return await database!.Table<Person>().Where(person => person.ID == id).FirstOrDefaultAsync();
	}

	public async Task<int> SavePersonAsync(Person person)
	{
		await Init();

		if (person.ID != 0)
		{
			return await database!.UpdateAsync(person);
		}

		return await database!.InsertAsync(person);
	}

	public async Task<int> DeletePersonAsync(Person person)
	{
		await Init();
		return await database!.DeleteAsync(person);
	}
}