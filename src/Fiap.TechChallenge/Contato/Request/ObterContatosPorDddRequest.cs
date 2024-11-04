namespace Fiap.TechChallenge.Contato.Request;

public class ObterContatosPorDddRequest
{
    public ObterContatosPorDddRequest(int ddd)
    {
        Ddd = ddd;
    }

    public int Ddd { get; set; }
}