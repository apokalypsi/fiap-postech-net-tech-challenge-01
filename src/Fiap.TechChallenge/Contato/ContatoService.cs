using Fiap.TechChallenge.Contato.Request;
using Fiap.TechChallenge.Contato.Result;
using Fiap.TechChallenge.DataBaseContext;
using Fiap.TechChallenge.Foundation.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Fiap.TechChallenge.Contato;

public class ContatoService : IContatoService
{
    private readonly IContatoCommandStore _contatoCommandStore;
    private readonly IContatoQueryStore _contatoQueryStore;
    private readonly ILogger<ContatoService> _logger;

    public ContatoService(ILogger<ContatoService> logger, IContatoCommandStore contatoCommandStore, IContatoQueryStore contatoQueryStore)
    {
        _logger = logger;
        _contatoCommandStore = contatoCommandStore;
        _contatoQueryStore = contatoQueryStore;
    }

    public async Task<CriarContatoResult> CriarContatoAsync(CriarContatoRequest request)
    {
        _logger.LogInformation("Iniciando criação de contato");
        try
        {
            // Validação de entrada
            ArgumentNullException.ThrowIfNull(request);

            // Verificar se o contato já existe
            var contatoExistente =
                await _contatoQueryStore.ContatoJaCadastradoAsync(request.Email, request.Telefone, request.DDD);
            if (contatoExistente) throw new BusinessException("Contato já cadastrado");

            // Criar entidade de contato
            var contato = new ContatoEntity(request.Nome, request.Telefone, request.Email, request.DDD);

            // Adicionar ao banco de dados
            var contatoAsync = await _contatoCommandStore.AdicionarContatoAsync(contato);

            return new CriarContatoResult { Contato = contatoAsync };
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar contato.");
            throw new Exception("Erro ao criar contato.");
        }
    }

    public async Task<AtualizarContatoResult> AtualizarContatoAsync(AtualizarContatoRequest request)
    {
        _logger.LogInformation("Iniciando atualização de contato");
        try
        {
            // Validação de entrada
            ArgumentNullException.ThrowIfNull(request);

            // Buscar o contato existente no banco de dados
            var contatoExistente = await _contatoQueryStore.ObterContatoPorIdAsync(request.Id);
            if (contatoExistente == null) throw new BusinessException("Contato não encontrado");

            // Atualizar as propriedades do contato com os novos valores do request
            contatoExistente.SetNome(request.Nome);
            contatoExistente.SetEmail(request.Email);
            contatoExistente.SetTelefone(request.Telefone);
            contatoExistente.SetDDD(request.DDD);

            // Atualizar o contato no banco de dados
            var contatoAtualizado = await _contatoCommandStore.AtualizarContatoAsync(contatoExistente);

            return new AtualizarContatoResult
            {
                Contato = contatoAtualizado
            };
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar contato.");
            throw new Exception("Erro ao atualizar contato.");
        }
    }

    public async Task<RemoverContatoResult> RemoverContatoAsync(RemoverContatoRequest request)
    {
        _logger.LogInformation("Iniciando remoção do contato com ID: {Id}", request.Id);

        try
        {
            // Tentar obter o contato para verificar se ele existe
            var contato = await _contatoQueryStore.ObterContatoPorIdAsync(request.Id);
            if (contato == null) throw new BusinessException("Contato não encontrado.");

            // Remover o contato do banco de dados
            var sucesso = await _contatoCommandStore.RemoverContatoAsync(request.Id);
            if (!sucesso) throw new Exception("Falha ao remover o contato.");

            // Retornar o resultado da remoção
            return new RemoverContatoResult
            {
                Sucesso = sucesso
            };
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, "Erro ao remover contato: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover contato: {Message}", ex.Message);
            throw new Exception("Erro ao remover contato.");
        }
    }

    public async Task<ObterContatoPorIdResult> ObterContatoPorIdAsync(ObterContatoPorIdRequest request)
    {
        _logger.LogInformation("Iniciando obtenção do contato com ID: {Id}", request.Id);
        try
        {
            // Consultar o contato no banco de dados
            var contato = await _contatoQueryStore.ObterContatoPorIdAsync(request.Id);
            if (contato == null) throw new BusinessException("Contato não encontrado.");

            // Mapeia os dados do contato para o resultado
            var resultado = new ObterContatoPorIdResult
            {
                Contato = new ContatoEntity(contato.Id, contato.Nome, contato.Telefone, contato.Email, contato.DDD)
            };

            return resultado;
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, "Erro ao obter contato: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter contato: {Message}", ex.Message);
            throw new Exception("Erro ao obter contato.");
        }
    }

    public async Task<ObterContatosPorDddResult> ObterContatosPorDddAsync(ObterContatosPorDddRequest request)
    {
        _logger.LogInformation("Iniciando a busca de contatos para o DDD: {Ddd}", request.Ddd);

        try
        {
            // Validação de entrada
            if (request.Ddd < 11 || request.Ddd > 99) throw new BusinessException("O DDD informado é inválido.");

            // Consultar os contatos no banco de dados com o DDD fornecido
            var contatos = await _contatoQueryStore.ObterContatosPorDddAsync(request.Ddd);

            if (contatos != null && contatos.Any()) return new ObterContatosPorDddResult(contatos.ToList());
            _logger.LogWarning("Nenhum contato encontrado para o DDD: {Ddd}", request.Ddd);
            return new ObterContatosPorDddResult(new List<ContatoEntity>());
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Erro ao obter contatos para o DDD: {Ddd}", request.Ddd);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter contatos para o DDD: {Ddd}", request.Ddd);
            throw new Exception("Erro ao obter contatos por DDD.");
        }
    }
}