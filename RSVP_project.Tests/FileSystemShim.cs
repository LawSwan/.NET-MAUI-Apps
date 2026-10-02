// Stand-in for MAUI's FileSystem so AppDatabase can run in a plain .NET test
// host. Each test run gets a brand-new folder, so the database starts empty
// and the first-run seed data is created fresh.
global using Microsoft.Maui.Storage;

namespace Microsoft.Maui.Storage;

public static class FileSystem
{
    public static string AppDataDirectory { get; } = CreateRunDirectory();

    static string CreateRunDirectory()
    {
        var dir = Path.Combine(Path.GetTempPath(), "rsvp-tests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        return dir;
    }
}
