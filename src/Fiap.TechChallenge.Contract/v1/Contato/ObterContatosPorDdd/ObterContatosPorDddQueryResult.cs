namespace Fiap.TechChallenge.Contract.v1.Contato.ObterContatosPorDdd;

public sealed class ObterContatosPorDddQueryResult
{
    /// <summary>
    ///     Lista de contatos retornados pela consulta.
    /// </summary>
    public List<ContatoQueryResult> Contatos { get; set; }
}