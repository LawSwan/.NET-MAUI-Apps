using SQLite;

namespace RSVPProject.Data;

/// <summary>
/// Local SQLite data access for the whole app. One connection, opened lazily
/// and shared by every page via <see cref="GetAsync"/>.
/// </summary>
public sealed class AppDatabase
{
    const string DbFileName = "rsvp.db3";

    static readonly Lazy<Task<AppDatabase>> LazyInstance = new(CreateAsync);

    /// <summary>Gets the shared, already-initialized database instance.</summary>
    public static Task<AppDatabase> GetAsync() => LazyInstance.Value;

    readonly SQLiteAsyncConnection connection;

    AppDatabase(SQLiteAsyncConnection connection) => this.connection = connection;

    static async Task<AppDatabase> CreateAsync()
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, DbFileName);
        var connection = new SQLiteAsyncConnection(path);
        var db = new AppDatabase(connection);
        await db.InitializeAsync();
        return db;
    }

    async Task InitializeAsync()
    {
        await connection.CreateTableAsync<UserRecord>();
        await connection.CreateTableAsync<EventRecord>();
        await connection.CreateTableAsync<RsvpRecord>();
        await SeedAsync();
    }

    // Populates a demo account + a few sample events on first run only, so
    // the app isn't empty out of the box and there's a login to test with.
    async Task SeedAsync()
    {
        if (await connection.Table<UserRecord>().CountAsync() > 0)
            return;

        var (hash, salt) = PasswordHasher.Hash("rsvp123");
        var demoUser = new UserRecord
        {
            FullName = "Amber Lawson",
            Email = "demo@rsvp.app",
            PasswordHash = hash,
            PasswordSalt = salt
        };
        await connection.InsertAsync(demoUser);

        var today = DateTime.Today;
        var filmNight = new EventRecord { Title = "Rooftop Film Night", StartsAt = today.AddDays(3).AddHours(19.5), Location = "The Lantern Rooftop", Description = "An open-air screening, warm blankets, and a small menu of late-summer snacks.", HostUserId = null, HostName = "Maya Chen" };
        var pastaClub = new EventRecord { Title = "Sunday Pasta Club", StartsAt = today.AddDays(5).AddHours(17), Location = "12 Olive Street", Description = "A relaxed evening of handmade pasta, shared plates, and good conversation.", HostUserId = demoUser.Id, HostName = demoUser.FullName };
        var gardenDay = new EventRecord { Title = "City Garden Volunteer Day", StartsAt = today.AddDays(11).AddHours(10), Location = "Northside Community Garden", Description = "Help refresh the garden beds and stay for coffee with the neighborhood crew.", HostUserId = null, HostName = "Jordan Lee" };
        var makersMarket = new EventRecord { Title = "Indie Makers Market", StartsAt = today.AddDays(12).AddHours(11), Location = "Foundry Hall", Description = "Local makers, prints, ceramics, and a corner for live acoustic sets.", HostUserId = null, HostName = "Maya Chen" };
        await connection.InsertAllAsync(new[] { filmNight, pastaClub, gardenDay, makersMarket });

        // Give the demo user an existing RSVP so "I'm attending" has something to show.
        await connection.InsertAsync(new RsvpRecord { EventId = filmNight.Id, UserId = demoUser.Id, GuestName = demoUser.FullName, GuestEmail = demoUser.Email, GuestCount = 1 });
    }

    // ---------------- Users ----------------

    // Emails are normalized to lowercase before every lookup/insert so login
    // is case-insensitive. The normalization happens in a local variable,
    // not inside the Where(...) lambda — sqlite-net's expression translator
    // only understands simple member comparisons, not arbitrary method calls.
    public Task<UserRecord?> GetUserByEmailAsync(string email)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return connection.Table<UserRecord>().Where(u => u.Email == normalized).FirstOrDefaultAsync()!;
    }

    public Task<UserRecord?> GetUserAsync(int id) =>
        connection.FindAsync<UserRecord>(id)!;

    /// <summary>Creates a new account. Throws if the email is already registered.</summary>
    public async Task<UserRecord> AddUserAsync(string fullName, string email, string password)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        if (await GetUserByEmailAsync(normalizedEmail) is not null)
            throw new InvalidOperationException("An account with that email already exists.");

        var (hash, salt) = PasswordHasher.Hash(password);
        var user = new UserRecord { FullName = fullName.Trim(), Email = normalizedEmail, PasswordHash = hash, PasswordSalt = salt };
        await connection.InsertAsync(user);
        return user;
    }

    /// <summary>Validates credentials against the stored hash. Returns null on failure.</summary>
    public async Task<UserRecord?> ValidateLoginAsync(string email, string password)
    {
        var user = await GetUserByEmailAsync(email);
        if (user is null) return null;
        return PasswordHasher.Verify(password, user.PasswordHash, user.PasswordSalt) ? user : null;
    }

    // ---------------- Events ----------------

    public Task<List<EventRecord>> GetAllEventsAsync() =>
        connection.Table<EventRecord>().OrderBy(e => e.StartsAt).ToListAsync();

    public Task<EventRecord?> GetEventAsync(int id) =>
        connection.FindAsync<EventRecord>(id)!;

    public Task<List<EventRecord>> GetEventsHostedByAsync(int userId) =>
        connection.Table<EventRecord>().Where(e => e.HostUserId == userId).OrderBy(e => e.StartsAt).ToListAsync();

    public async Task<List<EventRecord>> GetEventsAttendingAsync(int userId)
    {
        var rsvps = await connection.Table<RsvpRecord>().Where(r => r.UserId == userId).ToListAsync();
        var eventIds = rsvps.Select(r => r.EventId).ToHashSet();
        var all = await GetAllEventsAsync();
        return all.Where(e => eventIds.Contains(e.Id)).ToList();
    }

    public Task<int> AddEventAsync(EventRecord record) => connection.InsertAsync(record);

    // ---------------- RSVPs ----------------

    public Task<int> AddRsvpAsync(RsvpRecord record) => connection.InsertAsync(record);
}
