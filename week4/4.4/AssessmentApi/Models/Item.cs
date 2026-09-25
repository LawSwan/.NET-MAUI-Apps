using SQLite;

namespace AssessmentApi.Models;

/// <summary>
/// An item entered on the app's data entry page and stored in the SQLite database.
/// The JSON shape (id, name, description) matches the Item model in the MAUI app.
/// </summary>
public sealed class Item
{
    /// <summary>The user-entered item ID. It is the primary key, so each ID can only be saved once.</summary>
    [PrimaryKey]
    public string Id { get; set; } = string.Empty;

    /// <summary>The item's display name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>A longer description of the item.</summary>
    public string Description { get; set; } = string.Empty;
}
