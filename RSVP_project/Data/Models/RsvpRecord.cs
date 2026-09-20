using SQLite;

namespace RSVPProject.Data;

/// <summary>One RSVP against an event, stored in the "Rsvps" table.</summary>
public class RsvpRecord
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // FK -> Events.Id
    [NotNull, Indexed]
    public int EventId { get; set; }

    // FK -> Users.Id. Null for a guest RSVP (no account).
    [Indexed]
    public int? UserId { get; set; }

    [NotNull]
    public string GuestName { get; set; } = string.Empty;

    [NotNull]
    public string GuestEmail { get; set; } = string.Empty;

    public int GuestCount { get; set; } = 1;

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
