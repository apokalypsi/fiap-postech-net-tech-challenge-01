using System.Security.Cryptography;
using System.Text;

namespace Fiap.TechChallenge.Foundation.Core.Security;

/// <summary>
///     Represents the core configuration settings for security purposes.
/// </summary>
internal class SecurityServiceSettings
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SecurityServiceSettings" /> class with specified settings
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public SecurityServiceSettings()
    {
        var secretKeyString = Environment.GetEnvironmentVariable("FIdentity_SecretKey")!;
        var issuer = Environment.GetEnvironmentVariable("FIdentity_Issuer")!;
        var audience = Environment.GetEnvironmentVariable("FIdentity_Audience")!;
        var tokenExpirationInt = Convert.ToInt32(Environment.GetEnvironmentVariable("FIdentity_TokenExpiration")!);
        var refreshTokenExpirationInt =
            Convert.ToInt32(Environment.GetEnvironmentVariable("FIdentity_RefreshTokenExpiration")!);
        var privateKey = Environment.GetEnvironmentVariable("FIdentity_PrivateKey")!;

        var tokenExpiration = TimeSpan.FromMinutes(tokenExpirationInt);
        var refreshTokenExpiration = TimeSpan.FromMinutes(refreshTokenExpirationInt);

        if (string.IsNullOrWhiteSpace(secretKeyString))
            throw new ArgumentException("Key string cannot be null or whitespace.", nameof(secretKeyString));
        if (string.IsNullOrWhiteSpace(issuer))
            throw new ArgumentException("Issuer cannot be null or whitespace.", nameof(issuer));
        if (string.IsNullOrWhiteSpace(audience))
            throw new ArgumentException("Audience cannot be null or whitespace.", nameof(audience));
        if (secretKeyString.Length is < 36 or > 200)
            throw new ArgumentException("Key string must be between 36 and 200 characters long.",
                nameof(secretKeyString));
        if (string.IsNullOrWhiteSpace(privateKey))
            throw new ArgumentException("PrivateKey string cannot be null or whitespace.", nameof(privateKey));

        using var sha256 = SHA256.Create();
        SecretKey = sha256.ComputeHash(Encoding.UTF8.GetBytes(secretKeyString));
        RefreshTokenKey = sha256.ComputeHash(Encoding.UTF8.GetBytes(secretKeyString + "refresh"));
        Issuer = issuer;
        Audience = audience;
        TokenExpiration = tokenExpiration;
        RefreshTokenExpiration = refreshTokenExpiration; // Set the refresh token expiration time
        SecretKeyString = secretKeyString;
        PrivateKey = privateKey;
    }

    /// <summary>
    ///     Gets the secret key used for signing tokens. Should be stored securely.
    /// </summary>
    public byte[] SecretKey { get; private set; }

    public string SecretKeyString { get; private set; }

    public byte[] RefreshTokenKey { get; private set; }

    /// <summary>
    ///     Gets the issuer of the token.
    /// </summary>
    public string Issuer { get; private set; }

    /// <summary>
    ///     Gets the audience of the token.
    /// </summary>
    public string Audience { get; private set; }

    /// <summary>
    ///     Gets the expiration time of the access token.
    /// </summary>
    public TimeSpan TokenExpiration { get; private set; }

    /// <summary>
    ///     Gets the expiration time of the refresh token.
    /// </summary>
    public TimeSpan RefreshTokenExpiration { get; private set; }

    public string PrivateKey { get; private set; }
}