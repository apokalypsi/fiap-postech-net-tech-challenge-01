using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Fiap.TechChallenge.Foundation.Core.Enumerated;
using Fiap.TechChallenge.Foundation.Core.Exceptions;
using Fiap.TechChallenge.Foundation.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Fiap.TechChallenge.Foundation.Core.Security;

public class SecurityService : ISecurityService
{
    private readonly ILogger<SecurityService> _logger;
    private readonly SecurityServiceSettings _settings = new();

    public SecurityService(ILogger<SecurityService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Generates a JSON Web Token (JWT) for the given user.
    /// </summary>
    /// <param name="user">The UserEntity object containing user details and credentials.</param>
    /// <returns>A string representing the serialized JWT token for the user.</returns>
    /// <exception cref="InvalidOperationException">Thrown when there is an error in token generation.</exception>
    public async Task<string> GenerateTokenAsync(UserEntity user)
    {
        try
        {
            var result = await GenerateTokenAsync(user, ETokenType.AccessToken);
            return await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            // TODO: Add logging mechanism or error handling as per your application's requirements.
            _logger.LogError(ex, "Error generating JWT token for user {UserName}", user.UserName);
            throw new InvalidOperationException("Error generating JWT token", ex);
        }
    }


    /// <summary>
    ///     Generates a refresh token for a given user. This token is used to obtain a new access token
    ///     once the current access token expires.
    /// </summary>
    /// <param name="user">The UserEntity object representing the user for whom the refresh token is generated.</param>
    /// <returns>A string representing the serialized JWT refresh token for the user.</returns>
    /// <exception cref="InvalidOperationException">Thrown when there is an error in the refresh token generation process.</exception>
    public async Task<string> GenerateRefreshTokenAsync(UserEntity user)
    {
        try
        {
            var result = await GenerateTokenAsync(user, ETokenType.RefreshToken);
            return await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            // Logging the error occurred during the refresh token generation process.
            _logger.LogError(ex, "Error generating refresh token for user {UserName}", user.UserName);

            // Propagating the exception upwards after logging.
            throw new InvalidOperationException("Error generating refresh token", ex);
        }
    }

    /// <summary>
    ///     Extracts and returns a UserEntity from a given JWT token.
    /// </summary>
    /// <param name="token">The JWT token from which the user details are to be extracted.</param>
    /// <returns>A UserEntity object populated with user details extracted from the token.</returns>
    /// <exception cref="ArgumentException">Thrown when the token is null or empty.</exception>
    /// <exception cref="SecurityTokenException">Thrown when the token is invalid, expired, or not properly formatted.</exception>
    /// <exception cref="InvalidOperationException">Thrown for any other exceptions that occur during the process.</exception>
    public async Task<UserEntity> GetUserFromTokenAsync(string token, ETokenType tokenType)
    {
        if (string.IsNullOrEmpty(token))
        {
            _logger.LogWarning("Attempted to validate a null or empty token");
            throw new ArgumentException("Token cannot be null or empty.", nameof(token));
        }

        try
        {
            token = RemoveBearerPrefix(token);
            var tokenHandler = new JwtSecurityTokenHandler();

            // Creating a symmetric security key from the secret key in the settings.
            var key = tokenType switch
            {
                ETokenType.AccessToken => new SymmetricSecurityKey(_settings.SecretKey),
                ETokenType.RefreshToken => new SymmetricSecurityKey(_settings.RefreshTokenKey),
                _ => throw new ArgumentOutOfRangeException(nameof(tokenType), $"Unexpected token type: {tokenType}.")
            };

            // Validates the JWT token using the provided secret key and settings settings.
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _settings.Issuer,
                ValidateAudience = true,
                ValidAudience = _settings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            // Casting the validated token to JwtSecurityToken.
            var jwtToken = (JwtSecurityToken)validatedToken;
            var tokenClaims = jwtToken.Claims.ToList();

            // Extracting user information such as BusinessKey (UserId), username, roles, and policies from the token claims.
            var userId = Guid.Parse(tokenClaims.FirstOrDefault(claim => claim.Type == "UserId")?.Value!);
            var businessKey = tokenClaims.FirstOrDefault(claim => claim.Type == "BusinessKey")?.Value;
            var userName = tokenClaims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
            var roles = tokenClaims.Where(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value)
                .ToArray();
            var policies = tokenClaims.Where(claim => claim.Type == "Policy").Select(claim => claim.Value).ToArray();
            var email = tokenClaims.FirstOrDefault(claim => claim.Type == "Email")?.Value;

            // Ensuring userId is present and valid.
            if (string.IsNullOrEmpty(userId.ToString()))
                throw new SecurityTokenException("Token does not contain a valid UserId");

            // Returning a new UserEntity object with the extracted information.
            _logger.LogInformation("Token validation successful for token {Token}", token);
            return await Task.FromResult(new UserEntity(userId, businessKey, userName, email, roles, policies, true));
        }
        catch (SecurityTokenExpiredException ex)
        {
            _logger.LogError(ex, "Token is expired {Token}", token);
            throw new SecurityTokenException("Token is expired.", ex);
        }
        catch (SecurityTokenNotYetValidException ex)
        {
            _logger.LogError(ex, "Token is not yet valid. {Token}", token);
            throw new SecurityTokenException("Token is not yet valid.", ex);
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogError(ex, "Failed to validate token {Token}", token);
            throw new SecurityTokenException("Failed to validate the token due to a security token exception.", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            // Log e lançamento de exceção específica para acesso não autorizado.
            _logger.LogError(ex, "Unauthorized Access: Token does not contain the required policies or roles.");
            throw new SecurityTokenException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            // Catches other types of exceptions that might occur during token validation.
            _logger.LogError(ex, "Unexpected error occurred while validating token {Token}", token);
            throw new SecurityTokenException("Invalid token.", ex);
        }
    }

    /// <summary>
    ///     Encrypts the password using HMAC SHA256 hashing with a salt derived from the application's secret key.
    /// </summary>
    /// <param name="password">The password to encrypt.</param>
    /// <returns>The hashed password as a Base64 encoded string.</returns>
    /// <exception cref="ArgumentException">Thrown when the password is null or empty.</exception>
    public async Task<string> EncryptPasswordAsync(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            _logger.LogWarning("Attempted to encrypt a null or empty password");
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));
        }

        try
        {
            // Using HMACSHA256 for hashing the password with a key (salt).
            using (var hmac = new HMACSHA256(_settings.SecretKey))
            {
                // Convert the password string to a byte array.
                var passwordBytes = Encoding.UTF8.GetBytes(password);

                // Compute the hash of the password bytes.
                var hash = hmac.ComputeHash(passwordBytes);

                // Convert the hash byte array to a Base64 string and return it.
                _logger.LogInformation("Password encryption successful");
                return await Task.FromResult(Convert.ToBase64String(hash));
            }
        }
        catch (Exception ex)
        {
            // Handling any exceptions that might occur during the hashing process.
            _logger.LogError(ex, "Error occurred during password encryption");
            throw new InvalidOperationException("Error occurred during password encryption.", ex);
        }
    }

    /// <summary>
    ///     Generates a secure client secret.
    /// </summary>
    /// <returns>A randomly generated client secret as a hexadecimal string.</returns>
    public async Task<string> GenerateClientSecretAsync()
    {
        // Create a cryptographically secure random number generator
        using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
        {
            // Specify the desired length of the client secret in bytes
            var lengthInBytes = 128;

            // Generate random bytes
            var randomBytes = new byte[lengthInBytes];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            // Convert the random bytes to a hexadecimal string
            return await Task.FromResult(BitConverter.ToString(randomBytes).Replace("-", "").ToLower());
        }
    }

    /// <summary>
    ///     Validates a JWT token and retrieves user information, optionally verifying specified roles and policies.
    /// </summary>
    /// <param name="token">The JWT token to validate.</param>
    /// <param name="roles">
    ///     Optional. The roles expected to be present in the token. If provided, the token is checked to
    ///     ensure it contains these roles.
    /// </param>
    /// <param name="policies">
    ///     Optional. The policies expected to be present in the token. If provided, the token is checked to
    ///     ensure it contains these policies.
    /// </param>
    /// <returns>A UserEntity object populated with details extracted from the token.</returns>
    /// <exception cref="SecurityTokenException">
    ///     Thrown if the token is invalid, expired, or does not meet the required
    ///     validation criteria.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown if the token is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown for any other exceptions that occur during the process.</exception>
    public async Task<UserEntity> ValidateTokenAndRetrieveUserAsync(string token, IEnumerable<string> roles = null,
        IEnumerable<string> policies = null)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(_settings.SecretKey);

        try
        {
            token = RemoveBearerPrefix(token);
            // Validates the token with the specified parameters, ensuring the integrity and validity of the token.
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _settings.Issuer,
                ValidateAudience = true,
                ValidAudience = _settings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var tokenClaims = jwtToken.Claims.ToList();

            // Checks if roles and policies are provided, and validates them in the token.
            ValidateRolesAndPolicies(tokenClaims, roles, policies);

            // Extracts the BusinessKey, username, roles, and policies from the token claims.
            var userId = Guid.Parse(jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value!);
            var businessKey = tokenClaims.FirstOrDefault(c => c.Type == "BusinessKey")?.Value;
            var userName = tokenClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var userRoles = tokenClaims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
            var userPolicies = tokenClaims.Where(c => c.Type == "Policy").Select(c => c.Value).ToArray();
            var email = tokenClaims.FirstOrDefault(c => c.Type == "Email")?.Value;
            var multifator = Convert.ToBoolean(tokenClaims.FirstOrDefault(c => c.Type == "Multifator")?.Value);

            // Throws an exception if the userId is missing, ensuring the token contains necessary user identification.
            if (string.IsNullOrEmpty(userId.ToString()))
                throw new SecurityTokenException("Token does not contain a valid UserId");

            // Returns a new UserEntity object with the information extracted from the token.
            _logger.LogInformation("Token validation and user retrieval successful for token {Token}", token);
            return await Task.FromResult(new UserEntity(userId, businessKey, userName, email, userRoles, userPolicies,
                multifator));
        }
        catch (SecurityTokenExpiredException ex)
        {
            _logger.LogError(ex, "Token is expired {Token}", token);
            throw new SecurityTokenException("Token is expired.", ex);
        }
        catch (SecurityTokenNotYetValidException ex)
        {
            _logger.LogError(ex, "Token is not yet valid. {Token}", token);
            throw new SecurityTokenException("Token is not yet valid.", ex);
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogError(ex, "Failed to validate token {Token}", token);
            throw new SecurityTokenException("Failed to validate the token due to a security token exception.", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            // Log e lançamento de exceção específica para acesso não autorizado.
            _logger.LogError(ex, "Unauthorized Access: Token does not contain the required policies or roles.");
            throw new SecurityTokenException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            // Catches other types of exceptions that might occur during token validation.
            _logger.LogError(ex, "Unexpected error occurred while validating token {Token}", token);
            throw new SecurityTokenException("Invalid token.", ex);
        }
    }

    /// <summary>
    ///     Metodo para gerar a chave pública RSA a partir da chave privada.
    /// </summary>
    /// <returns></returns>
    public async Task<string> GenerateRsaPublicKeyAsync()
    {
        try
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_settings.PrivateKey);
            var publicKey = rsa.ToXmlString(false);
            return publicKey;
        }
        catch (Exception ex)
        {
            var message = $"Erro ao gerar chave pública RSA a partir da chave privada. Erro: {ex.Message}";
            _logger.LogError(ex, message);
            throw new SecurityTokenException(message, ex);
        }
    }

    /// <summary>
    ///     Metodo para gerar a chave privada RSA.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="SecurityTokenException"></exception>
    public async Task<string> GenerateRsaPrivateKeyAsync()
    {
        try
        {
            using var rsa = new RSACryptoServiceProvider(2048);
            return rsa.ToXmlString(true);
        }
        catch (Exception ex)
        {
            var message = $"Erro ao gerar chave pública RSA a partir da chave privada. Erro: {ex.Message}";
            _logger.LogError(ex, message);
            throw new SecurityTokenException(message, ex);
        }
    }

    /// <summary>
    ///     Metodo para criptografar conteúdo com chave pública RSA.
    /// </summary>
    /// <param name="publicKey"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="SecurityTokenException"></exception>
    public async Task<string> EncryptRsaAsync(string publicKey, string content)
    {
        try
        {
            if (string.IsNullOrEmpty(publicKey))
                throw new ArgumentException("Public key cannot be null or empty.", nameof(publicKey));
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException("Content cannot be null or empty.", nameof(content));

            byte[] data;
            var publicKeyString = "";

            try
            {
                data = Convert.FromBase64String(publicKey);
                publicKeyString = Encoding.UTF8.GetString(data);
            }
            catch (FormatException)
            {
                publicKeyString = publicKey;
            }

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKeyString);
                var encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(content), true);
                return Convert.ToBase64String(encryptedData);
            }
        }
        catch (Exception ex)
        {
            var message = $"Erro ao criptografar conteúdo com chave pública RSA. Erro: {ex.Message}";
            _logger.LogError(ex, message);
            throw new BusinessException(message, ex);
        }
    }

    /// <summary>
    ///     Metodo para descriptografar conteúdo com chave privada RSA.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="SecurityTokenException"></exception>
    public async Task<string> DecryptRsaAsync(string content)
    {
        try
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException("Content cannot be null or empty.", nameof(content));

            byte[] data;
            var privateKeyString = "";

            try
            {
                data = Convert.FromBase64String(_settings.PrivateKey);
                privateKeyString = Encoding.UTF8.GetString(data);
            }
            catch (FormatException)
            {
                privateKeyString = _settings.PrivateKey;
            }

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKeyString);
                var decryptedData = rsa.Decrypt(Convert.FromBase64String(content), true);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }
        catch (Exception ex)
        {
            var message = $"Erro ao descriptografar conteúdo com chave privada RSA. Erro: {ex.Message}";
            _logger.LogError(ex, message);
            throw new BusinessException(message, ex);
        }
    }

    /// <summary>
    ///     Refreshes a JWT access token using a valid refresh token.
    /// </summary>
    /// <param name="refreshToken">The valid JWT refresh token.</param>
    /// <returns>A new JWT access token string.</returns>
    /// <exception cref="SecurityTokenException">Thrown if the refresh token is invalid or expired.</exception>
    public async Task<string> RefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            _logger.LogWarning("Attempted to refresh a null or empty refresh token");
            throw new ArgumentException("Refresh token cannot be null or empty.", nameof(refreshToken));
        }

        try
        {
            // Extract the user details from the refresh token.
            var userEntity = await GetUserFromTokenAsync(refreshToken, ETokenType.RefreshToken);

            // Generate a new access token for the user.
            _logger.LogInformation("Access token refresh successful for refresh token {RefreshToken}", refreshToken);
            return await Task.FromResult(await GenerateTokenAsync(userEntity));
        }
        catch (SecurityTokenException ex)
        {
            // Handling exceptions related to refresh token validation.
            _logger.LogError(ex, "Access token refresh failed for refresh token {RefreshToken}", refreshToken);
            throw new SecurityTokenException("Access token refresh failed due to refresh token validation issues.", ex);
        }
        catch (Exception ex)
        {
            // Handling any other exceptions that might occur.
            _logger.LogError(ex,
                "Unexpected error occurred while refreshing access token using refresh token {RefreshToken}",
                refreshToken);
            throw new InvalidOperationException(
                "Error occurred while refreshing the access token using the refresh token.", ex);
        }
    }

    /// <summary>
    ///     Generates a JSON Web Token (JWT) for the given user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="tokenType"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<string> GenerateTokenAsync(UserEntity user, ETokenType tokenType)
    {
        try
        {
            // Creating a symmetric security key from the secret key in the settings.
            var key = tokenType switch
            {
                ETokenType.AccessToken => new SymmetricSecurityKey(_settings.SecretKey),
                ETokenType.RefreshToken => new SymmetricSecurityKey(_settings.RefreshTokenKey),
                _ => throw new ArgumentOutOfRangeException(nameof(tokenType), $"Unexpected token type: {tokenType}.")
            };

            var tokenExpiration = tokenType switch
            {
                ETokenType.AccessToken => _settings.TokenExpiration,
                ETokenType.RefreshToken => _settings.RefreshTokenExpiration,
                _ => throw new ArgumentOutOfRangeException(nameof(tokenType), $"Unexpected token type: {tokenType}.")
            };

            // Generating signing credentials using the security key with HMAC SHA256 algorithm.
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Preparing claims for the token. These include the user's username, a unique JWT ID, and the issue time.
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            // Adding role-based claims to the token.
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
            // Adding policy-based claims to the token.
            claims.AddRange(user.Policies.Select(policy => new Claim("Policy", policy)));
            // Additional claim specifically for the refresh process (if needed).
            claims.Add(new Claim("UserId", user.UserId.ToString()));
            claims.Add(new Claim("BusinessKey", user.BusinessKey));
            claims.Add(new Claim("TokenType", tokenType.ToString()));
            claims.Add(new Claim("Email", user.Email));
            claims.Add(new Claim("Multifator", user.Multifator.ToString()));

            // Creating the JWT token with issuer, audience, claims, expiration time, and signing credentials.
            var token = new JwtSecurityToken(
                _settings.Issuer,
                _settings.Audience,
                claims,
                expires: DateTime.UtcNow.Add(tokenExpiration),
                signingCredentials: credentials
            );

            // Returning the serialized JWT token.
            _logger.LogInformation("JWT token generated successfully for user {UserName}", user.UserName);
            var jwtSecurityToken = new JwtSecurityTokenHandler().WriteToken(token);
            return await Task.FromResult(jwtSecurityToken);
        }
        catch (Exception ex)
        {
            // TODO: Add logging mechanism or error handling as per your application's requirements.
            _logger.LogError(ex, "Error generating JWT token for user {UserName}", user.UserName);
            throw new InvalidOperationException("Error generating JWT token", ex);
        }
    }

    /// <summary>
    ///     Validates if the JWT token contains all the specified roles and policies.
    /// </summary>
    /// <param name="tokenClaims">The collection of claims from the JWT token.</param>
    /// <param name="roles">The required roles to check in the token.</param>
    /// <param name="policies">The required policies to check in the token.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown if the token lacks any of the required roles or policies.</exception>
    private void ValidateRolesAndPolicies(IEnumerable<Claim> tokenClaims, IEnumerable<string> roles,
        IEnumerable<string> policies)
    {
        try
        {
            // Garantindo que roles e policies não sejam nulos e removendo strings vazias ou espaços.
            roles = roles?.Where(role => !string.IsNullOrWhiteSpace(role)) ?? Enumerable.Empty<string>();
            policies = policies?.Where(policy => !string.IsNullOrWhiteSpace(policy)) ?? Enumerable.Empty<string>();

            // Extraindo roles dos token claims e verificando se todos os roles necessários estão presentes no token.
            var tokenRoles = tokenClaims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value.ToUpper());
            if (roles.Any() && !roles.All(role => tokenRoles.Contains(role.ToUpper())))
            {
                _logger.LogError("Token does not contain the required roles.");
                throw new UnauthorizedAccessException("Token does not contain the required roles.");
            }

            // Extraindo policies dos token claims e verificando se todas as policies necessárias estão presentes no token.
            var tokenPolicies = tokenClaims.Where(c => c.Type == "Policy").Select(c => c.Value.ToUpper());
            if (policies.Any() && !policies.All(policy => tokenPolicies.Contains(policy.ToUpper())))
            {
                _logger.LogError("Token does not contain the required policies.");
                throw new UnauthorizedAccessException("Token does not contain the required policies.");
            }
        }
        catch (UnauthorizedAccessException e)
        {
            // Log e lançamento de exceção específica para acesso não autorizado.
            _logger.LogError(e, "Unauthorized Access: Token does not contain the required policies or roles.");
            throw;
        }
        catch (Exception ex)
        {
            // Tratamento de exceções que podem ocorrer durante o processo de validação.
            _logger.LogError(ex, "Error occurred during roles and policies validation.");
            throw new InvalidOperationException("Error occurred during roles and policies validation.", ex);
        }
    }

    /// <summary>
    ///     Method to remove the "Bearer " prefix from the token string.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private string RemoveBearerPrefix(string token)
    {
        // Check if the input token starts with "Bearer " (case-insensitive)
        return token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ?
            // Remove "Bearer " prefix efficiently
            token.Substring(7)
            :
            // Token doesn't start with "Bearer ", return it as is
            token;
    }
}