namespace Fiap.TechChallenge.Foundation.Core.Languages;

public static class FormatarCodigoMensagem
{
    /// <summary>
    ///     Realiza a busca do código da mensagem "30|Mensagem Exemplo" retorna "30".
    /// </summary>
    /// <param name="mensagem"></param>
    /// <returns>Código informado na mensagem</returns>
    public static string ExtairCodigoMensagem(string mensagem)
    {
        if (!mensagem.Contains("|")) return "99999";
        var mensagenSeparada = mensagem.Split('|');
        var retorno = mensagenSeparada[0];
        return retorno;
    }

    /// <summary>
    ///     Realiza a busca da mensagem "30|Mensagem Exemplo" retorna "Mensagem Exemplo".
    /// </summary>
    /// <param name="mensagem"></param>
    /// <returns>Texto informado na mensagem</returns>
    public static string ExtairMensagem(string mensagem)
    {
        if (!mensagem.Contains("|")) return mensagem;
        var mensagenSeparada = mensagem.Split('|');
        var retorno = mensagenSeparada[1];
        return retorno;
    }
}