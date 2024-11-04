using Fiap.TechChallenge.CommandStore;
using Fiap.TechChallenge.DataBaseContext;
using Fiap.TechChallenge.Foundation.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fiap.TechChallenge.Tests.DataBase;

public class ContatoCommandStoreTests
{
    private static readonly Random _random = new();
    private readonly ContatoCommandStore _contatoCommandStore;
    private readonly AppDbContext _context;
    private readonly ILogger<ContatoCommandStore> _logger;
    private readonly IContatoQueryStore _queryStore;

    public ContatoCommandStoreTests()
    {
        // Configurar o DbContext para usar um banco de dados MySQL com a versão correta
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(
                "Server=mysql-fiap-techchallenge-54b6d47-alpha-5003.f.aivencloud.com;Port=12154;Database=defaultdb;User=avnadmin;Password=AVNS_z2edDgWWPxbU2kYW6o-;SslMode=Required;",
                new MySqlServerVersion(new Version(8, 0, 21))) // Coloque a versão correta do MySQL
            .Options;

        _context = new AppDbContext(options);
        _logger = new LoggerFactory().CreateLogger<ContatoCommandStore>();
        var queryLogger =
            new LoggerFactory().CreateLogger<ContatoQueryStore>(); // Cria logger correto para ContatoQueryStore

        _queryStore = new ContatoQueryStore(_context, queryLogger); // Passa o logger correto para ContatoQueryStore

        _contatoCommandStore = new ContatoCommandStore(_context, _logger, _queryStore);

        // Certificar-se de que o banco de dados está criado
        _context.Database.EnsureCreated();
    }

    private string GerarEmailAleatorio()
    {
        var guidEmail = Guid.NewGuid();
        return $"{new string(guidEmail.ToString())}@example.com";
    }

    private string GerarTelefoneAleatorio()
    {
        return $"9{_random.Next(10000000, 99999999)}"; // Gera um telefone aleatório
    }

    private int GerarDDDAleatorio()
    {
        return _random.Next(11, 99); // Gera um DDD entre 11 e 99
    }

    [Fact]
    public async Task AdicionarContatoAsync_DeveAdicionarContato_QuandoSucesso()
    {
        // Arrange
        var contato = new ContatoEntity(Guid.NewGuid(), "Diego", GerarTelefoneAleatorio(), GerarEmailAleatorio(),
            GerarDDDAleatorio());

        // Act
        var result = await _contatoCommandStore.AdicionarContatoAsync(contato);

        // Assert
        Assert.Equal(contato, result);

        // Verifica se o contato foi realmente adicionado no banco
        var contatoNoBanco = await _context.Contatos.FindAsync(contato.Id);
        Assert.NotNull(contatoNoBanco);
        Assert.Equal(contato.Nome, contatoNoBanco.Nome);
    }

    [Fact]
    public async Task AdicionarContatoAsync_DeveLancarException_QuandoContatoComMesmoEmailOuTelefoneExistir()
    {
        // Arrange
        var telefone = GerarTelefoneAleatorio();
        var email = GerarEmailAleatorio();
        var ddd = GerarDDDAleatorio();

        var contato1 = new ContatoEntity(Guid.NewGuid(), "Diego", telefone, email, ddd);
        await _context.Contatos.AddAsync(contato1);
        await _context.SaveChangesAsync();

        // Criação de um novo contato com o mesmo email e telefone/DDD
        var contatoDuplicado = new ContatoEntity(Guid.NewGuid(), "Outro Nome", telefone, email, ddd);

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<BusinessException>(() =>
                _contatoCommandStore.AdicionarContatoAsync(contatoDuplicado));
        Assert.Equal("Já existe um contato com o mesmo email ou telefone/DDD cadastrado.", exception.Message);
    }

    [Fact]
    public async Task AtualizarContatoAsync_DeveLancarException_QuandoOutroContatoComMesmoEmailOuTelefoneExistir()
    {
        // Arrange
        var contato1 = new ContatoEntity(Guid.NewGuid(), "Diego", GerarTelefoneAleatorio(), GerarEmailAleatorio(),
            GerarDDDAleatorio());
        await _context.Contatos.AddAsync(contato1);
        await _context.SaveChangesAsync();

        var contato2 = new ContatoEntity(Guid.NewGuid(), "Outro Nome", GerarTelefoneAleatorio(), GerarEmailAleatorio(),
            GerarDDDAleatorio());
        await _context.Contatos.AddAsync(contato2);
        await _context.SaveChangesAsync();

        // Tentar atualizar contato2 com o mesmo email e telefone/DDD de contato1
        contato2.SetEmail(contato1.Email);
        contato2.SetTelefone(contato1.Telefone);

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<BusinessException>(() => _contatoCommandStore.AtualizarContatoAsync(contato2));
        Assert.Equal("Já existe outro contato com o mesmo email ou telefone/DDD cadastrado.", exception.Message);
    }

    [Fact]
    public async Task AtualizarContatoAsync_DeveAtualizarContato_QuandoSucesso()
    {
        // Arrange
        var contato = new ContatoEntity(Guid.NewGuid(), "Diego", GerarTelefoneAleatorio(), GerarEmailAleatorio(),
            GerarDDDAleatorio());
        await _context.Contatos.AddAsync(contato);
        await _context.SaveChangesAsync();

        contato.SetNome("Diego Batista");

        // Act
        var result = await _contatoCommandStore.AtualizarContatoAsync(contato);

        // Assert
        Assert.Equal("Diego Batista", result.Nome);

        // Verifica se o contato foi atualizado no banco
        var contatoNoBanco = await _context.Contatos.FindAsync(contato.Id);
        Assert.Equal("Diego Batista", contatoNoBanco.Nome);
    }

    [Fact]
    public async Task RemoverContatoAsync_DeveRemoverContato_QuandoSucesso()
    {
        // Arrange
        var contato = new ContatoEntity(Guid.NewGuid(), "Diego", GerarTelefoneAleatorio(), GerarEmailAleatorio(),
            GerarDDDAleatorio());
        await _context.Contatos.AddAsync(contato);
        await _context.SaveChangesAsync();

        // Act
        var result = await _contatoCommandStore.RemoverContatoAsync(contato.Id);

        // Assert
        Assert.True(result);

        // Verifica se o contato foi removido do banco
        var contatoNoBanco = await _context.Contatos.FindAsync(contato.Id);
        Assert.Null(contatoNoBanco);
    }

    [Fact]
    public async Task RemoverContatoAsync_DeveRetornarFalse_QuandoContatoNaoExistir()
    {
        // Act
        var result = await _contatoCommandStore.RemoverContatoAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }
}