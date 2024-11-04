using Fiap.TechChallenge.DataBaseContext;

namespace Fiap.TechChallenge.Contato.Result;

public class ObterContatosPorDddResult
{
    public ObterContatosPorDddResult(List<ContatoEntity> contatos)
    {
        Contatos = contatos;
    }
    
    public List<ContatoEntity> Contatos { get; set; }
}