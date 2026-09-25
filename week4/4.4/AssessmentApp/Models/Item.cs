namespace AssessmentApp.Models;

/// <summary>
/// An item entered on the data entry page. Matches the JSON the web service sends and receives.
/// </summary>
public sealed class Item
{
    /// <summary>Unique item ID typed by the user.</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>The item's display name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>A longer description of the item.</summary>
    public string Description { get; set; } = string.Empty;
}
