using Fiap.TechChallenge.Foundation.Core.Enumerated;
using Fiap.TechChallenge.Foundation.Core.Models;

namespace Fiap.TechChallenge.Foundation.Core.Security;

public interface ISecurityService
{
    Task<string> GenerateTokenAsync(UserEntity user);
    Task<string> GenerateRefreshTokenAsync(UserEntity user);
    Task<string> RefreshTokenAsync(string refreshToken);
    Task<UserEntity> GetUserFromTokenAsync(string token, ETokenType tokenType);
    Task<string> EncryptPasswordAsync(string password);
    Task<string> GenerateClientSecretAsync();

    Task<UserEntity> ValidateTokenAndRetrieveUserAsync(string token, IEnumerable<string> roles,
        IEnumerable<string> policies);

    Task<string> GenerateRsaPublicKeyAsync();
    Task<string> GenerateRsaPrivateKeyAsync();
    Task<string> EncryptRsaAsync(string publicKey, string content);
    Task<string> DecryptRsaAsync(string content);
}