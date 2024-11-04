using Fiap.TechChallenge.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fiap.TechChallenge.Tests.DataBase;

public class ContatoQueryStoreTests
{
    private readonly ContatoQueryStore _contatoQueryStore;
    private readonly AppDbContext _context;
    private readonly ILogger<ContatoQueryStore> _logger;

    public ContatoQueryStoreTests()
    {
        // Configurar o DbContext para usar um banco de dados MySQL com a versão correta
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(
                "Server=mysql-fiap-techchallenge-54b6d47-alpha-5003.f.aivencloud.com;Port=12154;Database=defaultdb;User=avnadmin;Password=AVNS_z2edDgWWPxbU2kYW6o-;SslMode=Required;",
                new MySqlServerVersion(new Version(8, 0, 21))) // Coloque a versão correta do MySQL
            .Options;

        _context = new AppDbContext(options);
        _logger = new LoggerFactory().CreateLogger<ContatoQueryStore>();

        _contatoQueryStore = new ContatoQueryStore(_context, _logger);

        // Opcional: Certificar-se de que o banco de dados está criado
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task ObterTodosContatosAsync_DeveRetornarContatos()
    {
        // Arrange
        var contato = new ContatoEntity(Guid.NewGuid(), "Diego", "991662888", "diego@example.com", 48);
        await _context.Contatos.AddAsync(contato);
        await _context.SaveChangesAsync();

        // Act
        var result = await _contatoQueryStore.ObterTodosContatosAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Any());
    }

    [Fact]
    public async Task ObterContatosPorDddAsync_DeveRetornarContatos_QuandoDDDValido()
    {
        // Arrange
        var contato = new ContatoEntity(Guid.NewGuid(), "Diego", "991662888", "diego@example.com", 48);
        await _context.Contatos.AddAsync(contato);
        await _context.SaveChangesAsync();

        // Act
        var result = await _contatoQueryStore.ObterContatosPorDddAsync(48);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Any());
        Assert.All(result, c => Assert.Equal(48, c.DDD));
    }

    [Fact]
    public async Task ObterContatoPorIdAsync_DeveRetornarContato_QuandoIdValido()
    {
        // Arrange
        var contato = new ContatoEntity(Guid.NewGuid(), "Diego", "991662888", "diego@example.com", 48);
        await _context.Contatos.AddAsync(contato);
        await _context.SaveChangesAsync();

        // Act
        var result = await _contatoQueryStore.ObterContatoPorIdAsync(contato.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(contato.Id, result.Id);
    }

    [Fact]
    public async Task ObterContatoPorIdAsync_DeveRetornarNull_QuandoIdInvalido()
    {
        // Act
        var result = await _contatoQueryStore.ObterContatoPorIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }
}