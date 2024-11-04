namespace Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

public class CommandResult : ICommandResult
{
    // Mantendo o construtor vazio para necessidades de serialização ou herança.
    public CommandResult()
    {
    }

    // Construtor adicional para inicializar SourceId.
    public CommandResult(string sourceId) : this()
    {
        SourceId = sourceId;
    }

    public string SourceId { get; private set; } // Considerando um setter privado se necessário
    public bool Error { get; set; }
    public string? Code { get; set; }
    public string? Message { get; set; }

    public static CommandResult FromSource(string sourceId)
    {
        return new CommandResult(sourceId);
    }
}