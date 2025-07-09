using CreditScoringEngine.Application.Exceptions;
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
        Assert.True(result is not null);
    }

    [Fact]
    public async Task UpdateClient_DeveAtualizarCamposComSucesso()
    {
        using var context = new CreditScoringEngineContext(_options);
        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            Nome = "Original",
            Idade = 20,
            RendaMensal = 3000,
            HistoricoCreditoSimulado = "Médio"
        };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var service = new ClienteService(context);

        cliente.Nome = "Atualizado";
        cliente.Idade = 25;

        await service.UpdateAsync(cliente);

        var atualizado = await service.GetByIdAsync(cliente.Id);
        Assert.Equal("Atualizado", atualizado.Nome);
        Assert.Equal(25, atualizado.Idade);
    }

    [Fact]
    public async Task DeleteClient_DeveRemoverComSucesso()
    {
        using var context = new CreditScoringEngineContext(_options);
        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            Nome = "A Deletar",
            Idade = 40,
            RendaMensal = 4000,
            HistoricoCreditoSimulado = "Ruim"
        };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var service = new ClienteService(context);
        await service.DeleteAsync(cliente);

        
        var ex = await Assert.ThrowsAsync<ClienteNotFoundException>(async () =>
        {
            await service.GetByIdAsync(cliente.Id);
        });
        Assert.Contains("Cliente invalido, nao encontrado na base!", ex.Message);
    }

}
