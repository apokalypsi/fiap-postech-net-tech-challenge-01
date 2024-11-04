using Fiap.TechChallenge.Contract.v1.Contato.RemoverContato;
using FluentValidation.TestHelper;

namespace Fiap.TechChallenge.Tests.RemoverContato;

public class RemoverContatoCommandValidatorTests
{
    private readonly RemoverContatoCommandValidator _validator;

    public RemoverContatoCommandValidatorTests()
    {
        _validator = new RemoverContatoCommandValidator();
    }

    // Teste para o ID vazio
    [Fact]
    public void Deve_Falhar_Quando_Id_For_Vazio()
    {
        // Arrange
        var command = new RemoverContatoCommand { Id = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id).WithErrorMessage("O ID do contato é obrigatório.");
    }

    // Teste para o ID inválido (não é um GUID válido)
    [Fact]
    public void Deve_Falhar_Quando_Id_Nao_For_Guid()
    {
        // Arrange
        var command = new RemoverContatoCommand { Id = "12345" }; // ID inválido

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id).WithErrorMessage("O ID fornecido não é um GUID válido.");
    }

    // Teste para o ID válido (GUID válido)
    [Fact]
    public void Deve_Passar_Quando_Id_For_Valido()
    {
        // Arrange
        var command = new RemoverContatoCommand { Id = Guid.NewGuid().ToString() }; // ID válido (GUID)

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}