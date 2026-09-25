using SQLite;

namespace RemoteDataStorage.Models;

public class Person
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime DoB { get; set; }
}
