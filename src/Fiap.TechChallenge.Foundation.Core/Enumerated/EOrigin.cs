using System.ComponentModel.DataAnnotations;

namespace Fiap.TechChallenge.Foundation.Core.Enumerated;

public enum EOrigin
{
    [Display(Name = "CACHING")] Caching = 100,
    [Display(Name = "UNITFOUR")] Unitfour = 200,
    [Display(Name = "APICEP")] ApiCep = 300,
    [Display(Name = "AWESOMEAPI")] AwesomeApi = 400,
    [Display(Name = "AZUREMAPS")] AzureMaps = 500,
    [Display(Name = "GOOGLEMAPS")] GoogleMaps = 600,
    [Display(Name = "VIACEP")] ViaCep = 700,
    [Display(Name = "MINHARECEITA")] MinhaReceita = 800,
    [Display(Name = "BRASILAPI")] BrasilApi = 900
}