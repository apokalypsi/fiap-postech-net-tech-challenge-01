using Fiap.TechChallenge.Contato.Request;
using Fiap.TechChallenge.Contato.Result;

namespace Fiap.TechChallenge.Contato;

public interface IContatoService
{
    Task<CriarContatoResult> CriarContatoAsync(CriarContatoRequest request);
    Task<ObterContatoPorIdResult> ObterContatoPorIdAsync(ObterContatoPorIdRequest request);
    Task<ObterContatosPorDddResult> ObterContatosPorDddAsync(ObterContatosPorDddRequest request);
    Task<AtualizarContatoResult> AtualizarContatoAsync(AtualizarContatoRequest request);
    Task<RemoverContatoResult> RemoverContatoAsync(RemoverContatoRequest request);
}