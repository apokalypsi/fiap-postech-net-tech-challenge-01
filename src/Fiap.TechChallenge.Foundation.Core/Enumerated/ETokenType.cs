using System.ComponentModel.DataAnnotations;

namespace Fiap.TechChallenge.Foundation.Core.Enumerated;

public enum ETokenType
{
    [Display(Name = "ACCESSTOKEN")] AccessToken = 100,
    [Display(Name = "REFRESHTOKEN")] RefreshToken = 200
}