namespace MAUI_Data_Access.Models;

public static class DatabaseConstants
{
	public const string DatabaseFilename = "PeopleSQLite.db3";

	public const SQLite.SQLiteOpenFlags Flags =
		SQLite.SQLiteOpenFlags.ReadWrite |
		SQLite.SQLiteOpenFlags.Create |
		SQLite.SQLiteOpenFlags.SharedCache;

	public static string DatabasePath =>
		Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
}