﻿using Fiap.TechChallenge.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public sealed class ContatoQueryStore : IContatoQueryStore
{
    private readonly AppDbContext _context;
    private readonly ILogger<ContatoQueryStore> _logger;

    public ContatoQueryStore(AppDbContext context, ILogger<ContatoQueryStore> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    ///     Obtém todos os contatos do banco de dados.
    /// </summary>
    /// <returns>Retorna uma lista de todos os contatos cadastrados.</returns>
    public async Task<IEnumerable<ContatoEntity>> ObterTodosContatosAsync()
    {
        try
        {
            return await _context.Contatos.ToListAsync(); // Executa a consulta para obter todos os contatos.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter todos os contatos.");
            throw new Exception("Erro ao obter a lista de contatos. Tente novamente mais tarde.");
        }
    }

    /// <summary>
    ///     Obtém todos os contatos com base no DDD fornecido.
    /// </summary>
    /// <param name="ddd">DDD que será usado para filtrar os contatos.</param>
    /// <returns>Retorna uma lista de contatos associados ao DDD fornecido.</returns>
    public async Task<IEnumerable<ContatoEntity>> ObterContatosPorDddAsync(int ddd)
    {
        try
        {
            return await _context.Contatos
                .Where(c => c.DDD == ddd)
                .ToListAsync(); // Executa a consulta para filtrar contatos pelo DDD.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter contatos pelo DDD {Ddd}.", ddd);
            throw new Exception($"Erro ao buscar os contatos com DDD {ddd}. Tente novamente mais tarde.");
        }
    }

    /// <summary>
    ///     Obtém um contato específico pelo ID fornecido.
    /// </summary>
    /// <param name="id">ID do contato a ser obtido.</param>
    /// <returns>Retorna a entidade Contato, ou null se o contato não for encontrado.</returns>
    public async Task<ContatoEntity?> ObterContatoPorIdAsync(Guid id)
    {
        try
        {
            var contato = await _context.Contatos.FindAsync(id); // Busca o contato pelo ID.

            if (contato == null) _logger.LogWarning("Contato com ID {Id} não foi encontrado.", id);

            return contato; // Retorna o contato ou null.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter contato com ID {Id}.", id);
            throw new Exception("Erro ao buscar o contato. Tente novamente mais tarde.");
        }
    }

    /// <summary>
    ///     Verifica se já existe um contato com o mesmo email ou telefone/DDD registrado para outro usuário.
    /// </summary>
    /// <param name="email">Email do contato.</param>
    /// <param name="telefone">Telefone do contato.</param>
    /// <param name="ddd">DDD do contato.</param>
    /// <param name="id">ID opcional do contato atual (para exclusão ao atualizar).</param>
    /// <returns>Retorna true se existir outro contato com o mesmo email ou telefone/DDD, false caso contrário.</returns>
    public async Task<bool> ContatoJaCadastradoAsync(string email, string telefone, int ddd, Guid? id = null)
    {
        return await _context.Contatos.AnyAsync(c =>
            (c.Email == email || (c.Telefone == telefone && c.DDD == ddd)) && (!id.HasValue || c.Id != id.Value));
    }
}