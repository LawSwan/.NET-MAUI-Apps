using RSVPProject;
using RSVPProject.Data;
using Xunit;

namespace RSVPProject.Tests;

// AppState is static, so keep these tests from running in parallel.
[CollectionDefinition("App", DisableParallelization = true)]
public class AppCollection { }

[Collection("App")]
public class RsvpAppTests
{
    static string UniqueEmail(string prefix) => $"{prefix}.{Guid.NewGuid():N}@test.app";

    // TC-01: Password hashing
    [Fact]
    public void TC01_PasswordHasher_VerifiesCorrectPassword_RejectsWrong_AndSaltsEachHash()
    {
        var (hash1, salt1) = PasswordHasher.Hash("Sunny#2026");
        var (hash2, salt2) = PasswordHasher.Hash("Sunny#2026");

        Assert.True(PasswordHasher.Verify("Sunny#2026", hash1, salt1));
        Assert.False(PasswordHasher.Verify("sunny#2026", hash1, salt1));
        Assert.NotEqual(salt1, salt2);
        Assert.NotEqual(hash1, hash2);
        Assert.DoesNotContain("Sunny#2026", hash1);
    }

    // TC-02: First-run seed + demo login
    [Fact]
    public async Task TC02_FirstRun_SeedsDemoAccountAndEvents_AndDemoLoginSucceeds()
    {
        var db = await AppDatabase.GetAsync();

        var demo = await db.ValidateLoginAsync("demo@rsvp.app", "rsvp123");
        Assert.NotNull(demo);
        Assert.Equal("Amber Lawson", demo!.FullName);

        var titles = (await db.GetAllEventsAsync()).Select(e => e.Title).ToList();
        Assert.Contains("Rooftop Film Night", titles);
        Assert.Contains("Sunday Pasta Club", titles);
        Assert.Contains("City Garden Volunteer Day", titles);
        Assert.Contains("Indie Makers Market", titles);

        var attending = await db.GetEventsAttendingAsync(demo.Id);
        Assert.Contains(attending, e => e.Title == "Rooftop Film Night");
    }

    // TC-03: Register a new account, then log in (email case/whitespace-insensitive)
    [Fact]
    public async Task TC03_AddUser_ThenLoginWithDifferentCaseAndSpaces_Succeeds()
    {
        var db = await AppDatabase.GetAsync();
        var email = UniqueEmail("Jamie.Rivera");

        var created = await db.AddUserAsync("  Jamie Rivera ", email.ToUpperInvariant(), "Picnic!42");
        Assert.True(created.Id > 0);
        Assert.Equal("Jamie Rivera", created.FullName);
        Assert.Equal(email.ToLowerInvariant(), created.Email);
        Assert.NotEqual("Picnic!42", created.PasswordHash);

        var loggedIn = await db.ValidateLoginAsync("  " + email.ToUpperInvariant() + "  ", "Picnic!42");
        Assert.NotNull(loggedIn);
        Assert.Equal(created.Id, loggedIn!.Id);
    }

    // TC-04: Duplicate registration is rejected
    [Fact]
    public async Task TC04_AddUser_WithAlreadyRegisteredEmail_IsRejected()
    {
        var db = await AppDatabase.GetAsync();
        var email = UniqueEmail("dup");
        await db.AddUserAsync("First Person", email, "pass1234");

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => db.AddUserAsync("Second Person", email.ToUpperInvariant(), "other5678"));
        Assert.Equal("An account with that email already exists.", ex.Message);
    }

    // TC-05: Invalid credentials are refused
    [Fact]
    public async Task TC05_Login_WithWrongPasswordOrUnknownEmail_ReturnsNull()
    {
        var db = await AppDatabase.GetAsync();

        Assert.Null(await db.ValidateLoginAsync("demo@rsvp.app", "wrongpass"));
        Assert.Null(await db.ValidateLoginAsync("demo@rsvp.app", "RSVP123"));
        Assert.Null(await db.ValidateLoginAsync("nobody@nowhere.app", "rsvp123"));
    }

    // TC-06: Adding an event — stored, listed in date order, attributed to its host
    [Fact]
    public async Task TC06_AddEvent_AppearsInSortedListAndInHostsEvents()
    {
        var db = await AppDatabase.GetAsync();
        var host = await db.AddUserAsync("Host Person", UniqueEmail("host"), "hostpass1");

        var record = new EventRecord
        {
            Title = "Board Game Night",
            StartsAt = DateTime.Today.AddDays(1).AddHours(18),
            Location = "Community Room B",
            Description = "Bring a favourite game.",
            HostUserId = host.Id,
            HostName = host.FullName
        };
        await db.AddEventAsync(record);
        Assert.True(record.Id > 0);

        var all = await db.GetAllEventsAsync();
        Assert.Contains(all, e => e.Id == record.Id);
        Assert.Equal(all.OrderBy(e => e.StartsAt).Select(e => e.Id), all.Select(e => e.Id));

        var hosted = await db.GetEventsHostedByAsync(host.Id);
        Assert.Single(hosted);
        Assert.Equal("Board Game Night", hosted[0].Title);
    }

    // TC-07: Saving an RSVP links the event to the user's "attending" list
    [Fact]
    public async Task TC07_AddRsvp_ForLoggedInUser_ShowsEventInAttendingList_GuestRsvpDoesNot()
    {
        var db = await AppDatabase.GetAsync();
        var user = await db.AddUserAsync("Rsvp Tester", UniqueEmail("rsvp"), "rsvppass1");
        var garden = (await db.GetAllEventsAsync()).First(e => e.Title == "City Garden Volunteer Day");
        var market = (await db.GetAllEventsAsync()).First(e => e.Title == "Indie Makers Market");

        Assert.Empty(await db.GetEventsAttendingAsync(user.Id));

        await db.AddRsvpAsync(new RsvpRecord { EventId = garden.Id, UserId = user.Id, GuestName = user.FullName, GuestEmail = user.Email, GuestCount = 2 });
        await db.AddRsvpAsync(new RsvpRecord { EventId = market.Id, UserId = null, GuestName = "Walk-in Guest", GuestEmail = "guest@test.app", GuestCount = 1 });

        var attending = await db.GetEventsAttendingAsync(user.Id);
        Assert.Single(attending);
        Assert.Equal(garden.Id, attending[0].Id);
    }

    // TC-08: Session state for user / guest / log out
    [Fact]
    public void TC08_AppState_UserGuestAndLogoutTransitions()
    {
        AppState.StartUserSession(7, "Jamie Rivera", "jamie@test.app");
        Assert.True(AppState.IsLoggedIn);
        Assert.False(AppState.IsGuest);
        Assert.Equal(7, AppState.CurrentUserId);
        Assert.Equal("Jamie Rivera", AppState.DisplayName);

        AppState.StartGuestSession();
        Assert.False(AppState.IsLoggedIn);
        Assert.True(AppState.IsGuest);
        Assert.Null(AppState.CurrentUserId);
        Assert.Equal("Guest", AppState.DisplayName);

        AppState.LogOut();
        Assert.False(AppState.IsLoggedIn);
        Assert.False(AppState.IsGuest);
        Assert.Null(AppState.CurrentUserId);
        Assert.Equal(string.Empty, AppState.DisplayName);
        Assert.Equal(string.Empty, AppState.Email);
    }

    // TC-09: A user cannot RSVP to the same event twice
    [Fact]
    public async Task TC09_AddRsvp_SameUserSameEventTwice_OnlyOneRsvpStored()
    {
        var db = await AppDatabase.GetAsync();
        var user = await db.AddUserAsync("Double Booker", UniqueEmail("double"), "doublepass1");
        var pasta = (await db.GetAllEventsAsync()).First(e => e.Title == "Sunday Pasta Club");

        var rsvp = () => new RsvpRecord { EventId = pasta.Id, UserId = user.Id, GuestName = user.FullName, GuestEmail = user.Email, GuestCount = 1 };
        await db.AddRsvpAsync(rsvp());
        try { await db.AddRsvpAsync(rsvp()); } catch (Exception) { /* rejecting the duplicate is acceptable */ }

        // Count rows directly in the database file the app is using.
        var conn = new SQLite.SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, "rsvp.db3"));
        var stored = await conn.Table<RsvpRecord>().Where(r => r.EventId == pasta.Id && r.UserId == user.Id).CountAsync();
        await conn.CloseAsync();
        Assert.Equal(1, stored);
    }

    // TC-10 (regression for the TC-09 fix): guests have no account, so several guest RSVPs to one event must still save
    [Fact]
    public async Task TC10_AddRsvp_MultipleGuestsSameEvent_AllStored()
    {
        var db = await AppDatabase.GetAsync();
        var film = (await db.GetAllEventsAsync()).First(e => e.Title == "Rooftop Film Night");
        var tag = Guid.NewGuid().ToString("N");

        await db.AddRsvpAsync(new RsvpRecord { EventId = film.Id, UserId = null, GuestName = "Guest One " + tag, GuestEmail = "one@test.app", GuestCount = 1 });
        await db.AddRsvpAsync(new RsvpRecord { EventId = film.Id, UserId = null, GuestName = "Guest Two " + tag, GuestEmail = "two@test.app", GuestCount = 3 });

        var conn = new SQLite.SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, "rsvp.db3"));
        var guests = await conn.Table<RsvpRecord>().Where(r => r.EventId == film.Id && r.UserId == null).ToListAsync();
        await conn.CloseAsync();
        Assert.Equal(2, guests.Count(g => g.GuestName.EndsWith(tag)));
    }
}
