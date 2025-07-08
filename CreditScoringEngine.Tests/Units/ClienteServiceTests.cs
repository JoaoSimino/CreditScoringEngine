using CreditScoringEngine.Application.Services;
using CreditScoringEngine.Domain.Entities;
using CreditScoringEngine.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace MinimalAPIKickoff.Tests.Units;

public class ClienteServiceTests
{
    private readonly DbContextOptions<CreditScoringEngineContext> _options;

    public ClienteServiceTests()
    {
        _options = new DbContextOptionsBuilder<CreditScoringEngineContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
    }

    [Fact]
    public async Task AoCadastrarUserSemNomeDeveRetornarFalha()
    {
        // Arrange
        using var context = new CreditScoringEngineContext(_options);
        context.Clientes.Add(new Cliente
        {
            Nome = "Cliente Joao",
        });

        //act
        var ex = await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            await context.SaveChangesAsync(); 
        });
        var service = new ClienteService(context);

        // Assert
        Assert.Contains("Required properties", ex.Message);
    }

    [Fact]
    public async Task AoCadastrarUserComTodasInformacoesDeveRetornarSucesso()
    {
        // Arrange
        using var context = new CreditScoringEngineContext(_options);
        context.Clientes.Add(new Cliente {
            Id = Guid.NewGuid(),
            Nome = "João Victor",
            RendaMensal = 8500,
            Idade = 32,
            HistoricoCreditoSimulado = "Sem inadimplência nos últimos 12 meses"
        });
        context.SaveChanges();

        var service = new ClienteService(context);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.Single(result);
    }

}
