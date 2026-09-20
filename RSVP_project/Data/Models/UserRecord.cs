using SQLite;

namespace RSVPProject.Data;

/// <summary>A registered account, stored in the "Users" table.</summary>
public class UserRecord
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string FullName { get; set; } = string.Empty;

    // Login identifier; must be unique.
    [NotNull, Unique]
    public string Email { get; set; } = string.Empty;

    // Never store the raw password — PBKDF2 hash + its random salt (see PasswordHasher).
    [NotNull]
    public string PasswordHash { get; set; } = string.Empty;

    [NotNull]
    public string PasswordSalt { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
