using Fiap.TechChallenge.Contract.v1.Contato.ObterContatoPorId;
using FluentValidation.TestHelper;

namespace Fiap.TechChallenge.Tests.ObterContatoPorId;

public class ObterContatoPorIdQueryValidatorTests
{
    private readonly ObterContatoPorIdQueryValidator _validator;

    public ObterContatoPorIdQueryValidatorTests()
    {
        _validator = new ObterContatoPorIdQueryValidator();
    }

    [Fact]
    public void Deve_Passar_Quando_Id_For_Valido()
    {
        // Arrange
        var request = new ObterContatoPorIdQueryRequest
        {
            Id = Guid.NewGuid().ToString() // GUID válido
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Deve_Falhar_Quando_Id_For_Vazio()
    {
        // Arrange
        var request = new ObterContatoPorIdQueryRequest
        {
            Id = string.Empty // ID vazio
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id).WithErrorMessage("O ID do contato é obrigatório.");
    }

    [Fact]
    public void Deve_Falhar_Quando_Id_Nao_For_Guid()
    {
        // Arrange
        var request = new ObterContatoPorIdQueryRequest
        {
            Id = "123" // ID inválido (não é um GUID)
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id).WithErrorMessage("O ID fornecido não é um GUID válido.");
    }
}