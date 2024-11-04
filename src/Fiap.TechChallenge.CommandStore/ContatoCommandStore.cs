using Fiap.TechChallenge.DataBaseContext;
using Fiap.TechChallenge.Foundation.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fiap.TechChallenge.CommandStore;

public sealed class ContatoCommandStore : IContatoCommandStore
{
    private readonly IContatoQueryStore _contatoQueryStore;
    private readonly AppDbContext _context;
    private readonly ILogger<ContatoCommandStore> _logger;


    public ContatoCommandStore(AppDbContext context, ILogger<ContatoCommandStore> logger,
        IContatoQueryStore contatoQueryStore)
    {
        _context = context;
        _logger = logger;
        _contatoQueryStore = contatoQueryStore;
    }

    /// <summary>
    ///     Adiciona um novo contato ao sistema.
    /// </summary>
    /// <param name="contato">Entidade <see cref="ContatoEntity" /> contendo as informações do contato a ser adicionado.</param>
    /// <returns>Retorna a entidade <see cref="ContatoEntity" /> adicionada com sucesso.</returns>
    /// <exception cref="DbUpdateException">Lançada quando ocorre um erro durante a adição ao banco de dados.</exception>
    /// <exception cref="Exception">Lançada quando ocorre um erro desconhecido durante a adição do contato.</exception>
    public async Task<ContatoEntity> AdicionarContatoAsync(ContatoEntity contato)
    {
        try
        {
            // Verifica se já existe um contato com o mesmo email ou telefone/DDD
            if (await _contatoQueryStore.ContatoJaCadastradoAsync(contato.Email, contato.Telefone, contato.DDD))
                throw new BusinessException("Já existe um contato com o mesmo email ou telefone/DDD cadastrado.");

            await _context.Contatos.AddAsync(contato); // Adiciona o contato ao contexto
            await _context.SaveChangesAsync(); // Salva as mudanças no banco de dados
            return contato; // Retorna a entidade adicionada
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Erro BusinessException: {Message}", ex.Message);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Erro ao adicionar o contato. Detalhes: {Message}", ex.Message);
            throw new Exception("Erro ao adicionar o contato. Verifique os dados e tente novamente.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro desconhecido ao adicionar o contato. Detalhes: {Message}", ex.Message);
            throw new Exception("Ocorreu um erro ao adicionar o contato. Tente novamente mais tarde.");
        }
    }

    /// <summary>
    ///     Atualiza as informações de um contato existente.
    /// </summary>
    /// <param name="contato">Entidade <see cref="ContatoEntity" /> contendo as novas informações do contato a ser atualizado.</param>
    /// <returns>Retorna a entidade <see cref="ContatoEntity" /> atualizada com sucesso.</returns>
    /// <exception cref="DbUpdateConcurrencyException">
    ///     Lançada quando ocorre um erro de concorrência, ou seja, o contato foi
    ///     modificado por outra pessoa.
    /// </exception>
    /// <exception cref="DbUpdateException">Lançada quando ocorre um erro durante a atualização no banco de dados.</exception>
    /// <exception cref="Exception">Lançada quando ocorre um erro desconhecido durante a atualização do contato.</exception>
    public async Task<ContatoEntity> AtualizarContatoAsync(ContatoEntity contato)
    {
        try
        {
            // Verifica se já existe outro contato com o mesmo email ou telefone/DDD
            if (await _contatoQueryStore.ContatoJaCadastradoAsync(contato.Email, contato.Telefone, contato.DDD,
                    contato.Id))
                throw new BusinessException("Já existe outro contato com o mesmo email ou telefone/DDD cadastrado.");

            _context.Contatos.Update(contato); // Atualiza a entidade no contexto
            await _context.SaveChangesAsync(); // Salva as mudanças no banco de dados
            return contato; // Retorna a entidade atualizada
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Erro BusinessException: {Message}", ex.Message);
            throw;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Erro de concorrência ao atualizar o contato.");
            throw new Exception("O contato foi modificado por outra pessoa. Tente novamente.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Erro ao atualizar o contato.");
            throw new Exception("Erro ao atualizar o contato. Verifique os dados e tente novamente.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro desconhecido ao atualizar o contato.");
            throw new Exception("Ocorreu um erro ao atualizar o contato. Tente novamente mais tarde.");
        }
    }

    /// <summary>
    ///     Remove um contato existente com base no ID fornecido.
    /// </summary>
    /// <param name="id">Identificador único (GUID) do contato a ser removido.</param>
    /// <returns>
    ///     Retorna <see cref="bool" /> indicando se a remoção foi bem-sucedida (true) ou se o contato não foi encontrado
    ///     (false).
    /// </returns>
    /// <exception cref="DbUpdateException">Lançada quando ocorre um erro durante a remoção no banco de dados.</exception>
    /// <exception cref="Exception">Lançada quando ocorre um erro desconhecido durante a remoção do contato.</exception>
    public async Task<bool> RemoverContatoAsync(Guid id)
    {
        try
        {
            var contato = await _contatoQueryStore.ObterContatoPorIdAsync(id);
            if (contato != null)
            {
                _context.Contatos.Remove(contato);
                await _context.SaveChangesAsync();
                return true; // Retorna true indicando que a remoção foi bem-sucedida
            }

            _logger.LogWarning("Tentativa de remoção falhou. Contato com o ID {Id} não foi encontrado.", id);
            return false; // Retorna false se o contato não foi encontrado
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Erro ao remover o contato com ID {Id}.", id);
            throw new Exception("Erro ao remover o contato. Tente novamente mais tarde.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro desconhecido ao remover o contato com ID {Id}.", id);
            throw new Exception("Ocorreu um erro ao remover o contato. Tente novamente mais tarde.");
        }
    }
}