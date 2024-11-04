using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Fiap.TechChallenge.Foundation.Core.Languages;

namespace Fiap.TechChallenge.Foundation.Core.Extensions;

public static class StringExtensions
{
    public static string Args(this string format, params object[] args)
    {
        return string.Format(format, args);
    }

    /// <summary>
    ///     Realiza a busca do código da mensagem "30|Mensagem Exemplo" retorna "30".
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string ToCode(this string message)
    {
        return FormatarCodigoMensagem.ExtairCodigoMensagem(message);
    }

    /// <summary>
    ///     Realiza a busca da mensagem "30|Mensagem Exemplo" retorna "Mensagem Exemplo"
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string ToMessage(this string message)
    {
        return FormatarCodigoMensagem.ExtairMensagem(message);
    }

    /// <summary>
    ///     Validar CPF
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsCpf(this string value)
    {
        try
        {
            var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            value = string.Join("", value.ToCharArray().Where(char.IsDigit));

            if (value.Length != 11)
                return false;

            for (var j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == value)
                    return false;

            var tempCpf = value.Substring(0, 9);
            var soma = 0;

            for (var i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            var resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            var digito = resto.ToString();
            tempCpf += digito;
            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto;

            return value.EndsWith(digito);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public static bool IsCnpj(this string value)
    {
        var cnpj = string.Join("", value.ToCharArray().Where(char.IsDigit));

        int[] digitos, soma, resultado;
        int nrDig;
        string ftmt;
        bool[] cnpjOk;

        ftmt = "6543298765432";
        digitos = new int[14];
        soma = new int[2];
        soma[0] = 0;
        soma[1] = 0;
        resultado = new int[2];
        resultado[0] = 0;
        resultado[1] = 0;
        cnpjOk = new bool[2];
        cnpjOk[0] = false;
        cnpjOk[1] = false;

        try

        {
            for (nrDig = 0; nrDig < 14; nrDig++)
            {
                digitos[nrDig] = int.Parse(cnpj.Substring(nrDig, 1));

                if (nrDig <= 11)
                    soma[0] += digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1));

                if (nrDig <= 12)
                    soma[1] += digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1));
            }


            for (nrDig = 0; nrDig < 2; nrDig++)
            {
                resultado[nrDig] = soma[nrDig] % 11;

                if (resultado[nrDig] == 0 || resultado[nrDig] == 1)
                    cnpjOk[nrDig] = digitos[12 + nrDig] == 0;
                else
                    cnpjOk[nrDig] = digitos[12 + nrDig] == 11 - resultado[nrDig];
            }

            return cnpjOk[0] && cnpjOk[1];
        }
        catch
        {
            return false;
        }
    }

    public static string GetEnumDisplayName(this Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());
        var attributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);
        return attributes.Length > 0 ? attributes[0].Name : value.ToString();
    }

    public static string RemoveAccents(this string text)
    {
        return new string(text
            .Normalize(NormalizationForm.FormD)
            .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
            .ToArray());
    }
}