using SQLite;

namespace RSVPProject.Data;

/// <summary>An event, stored in the "Events" table.</summary>
public class EventRecord
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string Title { get; set; } = string.Empty;

    // Date and start time combined into one column.
    [NotNull]
    public DateTime StartsAt { get; set; }

    [NotNull]
    public string Location { get; set; } = string.Empty;

    [NotNull]
    public string Description { get; set; } = string.Empty;

    // FK -> Users.Id. Null when the event was created by a guest (no account).
    [Indexed]
    public int? HostUserId { get; set; }

    // Denormalized snapshot of the host's display name, so the list/detail
    // screens can show a host even for guest-created events (no HostUserId).
    [NotNull]
    public string HostName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Ignore]
    public string DateDisplay => StartsAt.ToString("ddd, MMM d");

    [Ignore]
    public string TimeDisplay => StartsAt.ToString("h:mm tt");
}
