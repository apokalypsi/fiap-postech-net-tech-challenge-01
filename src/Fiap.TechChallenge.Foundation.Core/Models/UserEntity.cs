namespace Fiap.TechChallenge.Foundation.Core.Models;

/// <summary>
///     Representa um usuário com roles, políticas e uma chave de negócio única.
/// </summary>
public class UserEntity
{
    /// <summary>
    ///     Inicializa uma nova instância da classe <see cref="UserEntity" />.
    /// </summary>
    /// <param name="userId">O ID único do usuário.</param>
    /// <param name="businessKey">A chave de negócio única para o usuário (ex.: CPF).</param>
    /// <param name="userName">O nome de usuário.</param>
    /// <param name="email">O e-mail do usuário.</param>
    /// <param name="roles">As roles atribuídas ao usuário.</param>
    /// <param name="policies">As políticas associadas ao usuário.</param>
    /// <param name="multifator">Indica se a autenticação multifator está habilitada para o usuário.</param>
    public UserEntity(Guid userId, string businessKey, string userName, string email, IEnumerable<string> roles,
        IEnumerable<string> policies, bool multifator)
    {
        UserId = userId;
        BusinessKey = businessKey ?? throw new ArgumentNullException(nameof(businessKey));
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Roles = new List<string>(roles ?? throw new ArgumentNullException(nameof(roles)));
        Policies = new List<string>(policies ?? throw new ArgumentNullException(nameof(policies)));
        Multifator = multifator; // Inicialização correta da propriedade Multifator.
    }


    /// <summary>
    ///     Obtém o ID único do usuário.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    ///     Obtém a chave de negócio do usuário.
    /// </summary>
    public string BusinessKey { get; }

    /// <summary>
    ///     Obtém o nome de usuário.
    /// </summary>
    public string UserName { get; }

    /// <summary>
    ///     Obtém o e-mail de usuário.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///     Obtém as roles atribuídas ao usuário.
    /// </summary>
    public IReadOnlyList<string> Roles { get; }

    /// <summary>
    ///     Obtém as políticas associadas ao usuário.
    /// </summary>
    public IReadOnlyList<string> Policies { get; }

    /// <summary>
    ///     Obtém se a autenticação multifator está habilitada para o usuário
    /// </summary>
    public bool Multifator { get; }

    /// <summary>
    ///     Retorna uma string que representa o objeto atual.
    /// </summary>
    /// <returns>Uma string que representa o objeto atual.</returns>
    public override string ToString()
    {
        return
            $"UserId: {UserId}, BusinessKey: {BusinessKey}, UserName: {UserName}, Email: {Email}, Roles: [{string.Join(", ", Roles)}], Policies: [{string.Join(", ", Policies)}], Multifator: {Multifator}";
    }
}