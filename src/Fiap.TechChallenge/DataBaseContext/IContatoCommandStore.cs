namespace Fiap.TechChallenge.DataBaseContext;

public interface IContatoCommandStore
{
    /// <summary>
    ///     Adiciona um novo contato e retorna a entidade criada.
    /// </summary>
    /// <param name="contato">A entidade Contato a ser adicionada.</param>
    /// <returns>O contato adicionado.</returns>
    Task<ContatoEntity> AdicionarContatoAsync(ContatoEntity contato);

    /// <summary>
    ///     Atualiza um contato existente e retorna a entidade atualizada.
    /// </summary>
    /// <param name="contato">A entidade Contato com os novos dados a serem atualizados.</param>
    /// <returns>O contato atualizado.</returns>
    Task<ContatoEntity> AtualizarContatoAsync(ContatoEntity contato);

    /// <summary>
    ///     Remove um contato pelo seu ID.
    /// </summary>
    /// <param name="id">O identificador único do contato a ser removido.</param>
    /// <returns>Uma tarefa assíncrona.</returns>
    Task<bool> RemoverContatoAsync(Guid id);
}