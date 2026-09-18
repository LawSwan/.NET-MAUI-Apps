using SQLite;

namespace ItemCatalog.Models;

public class Item
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string ItemId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}