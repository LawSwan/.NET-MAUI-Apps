using AssessmentApi.Models;
using SQLite;

namespace AssessmentApi.DataAccess;

/// <summary>
/// Reads and writes items in the SQLite database. Registered as a singleton so the
/// whole API shares one database connection.
/// </summary>
public sealed class ItemData
{
    private SQLiteConnection? database;

    /// <summary>
    /// Opens the database connection and creates the Item table the first time it is needed.
    /// </summary>
    private SQLiteConnection Init()
    {
        if (database is not null)
        {
            return database;
        }

        database = new SQLiteConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
        database.CreateTable<Item>();
        return database;
    }

    /// <summary>
    /// Returns every stored item, sorted by item ID.
    /// </summary>
    public List<Item> GetItems()
    {
        return Init().Table<Item>().OrderBy(item => item.Id).ToList();
    }

    /// <summary>
    /// Inserts a new item.
    /// </summary>
    /// <param name="item">The item to store.</param>
    /// <returns>True if the item was saved; false if an item with the same ID already exists.</returns>
    public bool AddItem(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);
        var connection = Init();

        if (connection.Find<Item>(item.Id) is not null)
        {
            return false;
        }

        connection.Insert(item);
        return true;
    }
}
