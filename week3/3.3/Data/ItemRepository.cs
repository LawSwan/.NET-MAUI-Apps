using ItemCatalog.Models;
using SQLite;

namespace ItemCatalog.Data;

public class ItemRepository
{
    private readonly SQLiteConnection _database;

    public ItemRepository()
    {
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "items.db3");
        _database = new SQLiteConnection(databasePath);
        _database.CreateTable<Item>();
    }

    public void AddItem(Item item)
    {
        _database.Insert(item);
    }

    public List<Item> GetAllItems()
    {
        return _database.Table<Item>()
            .OrderBy(item => item.Id)
            .ToList();
    }
}