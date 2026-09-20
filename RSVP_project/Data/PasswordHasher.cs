using System.Security.Cryptography;

namespace RSVPProject.Data;

/// <summary>
/// Salted PBKDF2 password hashing, so accounts never store (or compare
/// against) a plain-text password.
/// </summary>
internal static class PasswordHasher
{
    const int SaltSize = 16;
    const int HashSize = 32;
    const int Iterations = 100_000;

    public static (string Hash, string Salt) Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
        return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    public static bool Verify(string password, string storedHash, string storedSalt)
    {
        var salt = Convert.FromBase64String(storedSalt);
        var computed = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
        return CryptographicOperations.FixedTimeEquals(computed, Convert.FromBase64String(storedHash));
    }
}
