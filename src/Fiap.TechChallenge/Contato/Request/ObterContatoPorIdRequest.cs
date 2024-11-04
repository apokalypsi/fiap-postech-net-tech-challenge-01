namespace Fiap.TechChallenge.Contato.Request;

public class ObterContatoPorIdRequest
{
    public ObterContatoPorIdRequest(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}