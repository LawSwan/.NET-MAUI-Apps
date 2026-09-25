using RemoteDataStorage.Models;
using SQLite;

namespace RemoteDataStorage.DataAccess;

public class PersonData
{
    private SQLiteConnection? database;

    private void Init()
    {
        if (database is not null)
        {
            return;
        }

        database = new SQLiteConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
        database.CreateTable<Person>();
    }

    public List<Person> GetPeople()
    {
        Init();
        return database!.Table<Person>().ToList();
    }

    public Person? GetPerson(int id)
    {
        Init();
        return database!.Table<Person>().FirstOrDefault(person => person.ID == id);
    }

    public int SavePerson(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);
        Init();

        return person.ID != 0
            ? database!.Update(person)
            : database!.Insert(person);
    }

    public int DeletePerson(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);
        Init();
        return database!.Delete(person);
    }
}