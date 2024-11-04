using Fiap.TechChallenge.Contract.v1.Contato.AtualizarContato;
using FluentValidation.TestHelper;

namespace Fiap.TechChallenge.Tests.AtualizarContato;

public class AtualizarContatoCommandValidatorTests
{
    private readonly AtualizarContatoCommandValidator _validator;

    public AtualizarContatoCommandValidatorTests()
    {
        _validator = new AtualizarContatoCommandValidator();
    }

    // Teste para o ID (validar GUID)
    [Fact]
    public void Deve_Falhar_Quando_Id_For_Vazio()
    {
        // Arrange
        var command = new AtualizarContatoCommand { Id = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id).WithErrorMessage("O ID do contato é obrigatório.");
    }

    [Fact]
    public void Deve_Falhar_Quando_Id_Nao_For_Guid()
    {
        // Arrange
        var command = new AtualizarContatoCommand { Id = "12345" }; // ID inválido

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id).WithErrorMessage("O ID fornecido não é um GUID válido.");
    }

    [Fact]
    public void Deve_Passar_Quando_Id_For_Valido()
    {
        // Arrange
        var command = new AtualizarContatoCommand { Id = Guid.NewGuid().ToString() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    // Testes para o Nome
    [Fact]
    public void Deve_Falhar_Quando_Nome_For_Vazio()
    {
        // Arrange
        var command = new AtualizarContatoCommand { Nome = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nome).WithErrorMessage("O nome do contato é obrigatório.");
    }

    [Fact]
    public void Deve_Falhar_Quando_Nome_For_Menor_Que_Dois_Caracteres()
    {
        // Arrange
        var command = new AtualizarContatoCommand { Nome = "A" }; // Nome com menos de 2 caracteres

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nome).WithErrorMessage("O nome deve ter no mínimo 2 caracteres.");
    }

    [Fact]
    public void Deve_Falhar_Quando_Nome_For_Maior_Que_100_Caracteres()
    {
        // Arrange
        var command = new AtualizarContatoCommand { Nome = new string('A', 101) }; // Nome com mais de 100 caracteres

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nome).WithErrorMessage("O nome pode ter no máximo 100 caracteres.");
    }

    [Fact]
    public void Deve_Passar_Quando_Nome_For_Valido()
    {
        // Arrange
        var command = new AtualizarContatoCommand { Nome = "Diego" }; // Nome válido

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Nome);
    }

    // Testes para o Telefone
    [Fact]
    public void Deve_Falhar_Quando_Telefone_For_Vazio()
    {
        // Arrange
        var command = new AtualizarContatoCommand { Telefone = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Telefone).WithErrorMessage("O telefone do contato é obrigatório.");
    }

    [Fact]
    public void Deve_Falhar_Quando_Telefone_Nao_Tiver_Entre_8_E_15_Digitos()
    {
        // Arrange
        var command = new AtualizarContatoCommand { Telefone = "123456" }; // Telefone inválido

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Telefone)
            .WithErrorMessage("O telefone deve ter entre 8 e 15 dígitos.");
    }

    [Fact]
    public void Deve_Passar_Quando_Telefone_For_Valido()
    {
        // Arrange
        var command = new AtualizarContatoCommand { Telefone = "123456789" }; // Telefone válido

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Telefone);
    }

    // Testes para o Email
    [Fact]
    public void Deve_Falhar_Quando_Email_For_Vazio()
    {
        // Arrange
        var command = new AtualizarContatoCommand { Email = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("O e-mail do contato é obrigatório.");
    }

    [Fact]
    public void Deve_Falhar_Quando_Email_For_Invalido()
    {
        // Arrange
        var command = new AtualizarContatoCommand { Email = "email_invalido" }; // Email inválido

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("O e-mail fornecido não é válido.");
    }

    [Fact]
    public void Deve_Passar_Quando_Email_For_Valido()
    {
        // Arrange
        var command = new AtualizarContatoCommand { Email = "email@exemplo.com" }; // Email válido

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    // Testes para o DDD
    [Fact]
    public void Deve_Falhar_Quando_DDD_For_Invalido()
    {
        // Arrange
        var command = new AtualizarContatoCommand { DDD = 123 }; // DDD inválido

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DDD)
            .WithErrorMessage("O DDD deve ser composto por dois dígitos entre 11 e 99.");
    }

    [Fact]
    public void Deve_Passar_Quando_DDD_For_Valido()
    {
        // Arrange
        var command = new AtualizarContatoCommand { DDD = 11 }; // DDD válido

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.DDD);
    }
}