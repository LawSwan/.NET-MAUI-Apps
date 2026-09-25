using SQLite;

namespace AssessmentApi.Models;

/// <summary>
/// Settings for the SQLite database file that stores the items.
/// </summary>
public static class DatabaseConstants
{
    /// <summary>Name of the database file created next to the API's compiled output.</summary>
    public const string DatabaseFilename = "ItemsSQLite.db3";

    /// <summary>
    /// Open the database for reading and writing, create it if it does not exist, and
    /// serialize access so the singleton connection is safe to use from concurrent requests.
    /// </summary>
    public const SQLiteOpenFlags Flags =
        SQLiteOpenFlags.ReadWrite |
        SQLiteOpenFlags.Create |
        SQLiteOpenFlags.FullMutex;

    /// <summary>Full path to the database file.</summary>
    public static string DatabasePath =>
        Path.Combine(AppContext.BaseDirectory, DatabaseFilename);
}
