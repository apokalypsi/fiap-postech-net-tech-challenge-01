using Fiap.TechChallenge.Contract.v1.Contato.ObterContatosPorDdd;
using FluentValidation.TestHelper;

namespace Fiap.TechChallenge.Tests.ObterContatosPorDdd;

public class ObterContatosPorDddQueryValidatorTests
{
    private readonly ObterContatosPorDddQueryValidator _validator;

    public ObterContatosPorDddQueryValidatorTests()
    {
        _validator = new ObterContatosPorDddQueryValidator();
    }

    [Fact]
    public void Deve_Passar_Quando_Ddd_For_Valido()
    {
        // Arrange
        var request = new ObterContatosPorDddQueryRequest
        {
            Ddd = 11 // DDD válido
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Deve_Falhar_Quando_Ddd_For_Vazio()
    {
        // Arrange
        var request = new ObterContatosPorDddQueryRequest
        {
            Ddd = 0 // DDD inválido (vazio)
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Ddd).WithErrorMessage("O DDD é obrigatório.");
    }

    [Fact]
    public void Deve_Falhar_Quando_Ddd_For_Invalido()
    {
        // Arrange
        var request = new ObterContatosPorDddQueryRequest
        {
            Ddd = 100 // DDD inválido (fora do intervalo de 11 a 99)
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Ddd)
            .WithErrorMessage("O DDD deve ser composto por dois dígitos entre 11 e 99.");
    }
}