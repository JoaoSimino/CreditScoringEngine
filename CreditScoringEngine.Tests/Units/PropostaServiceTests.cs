using CreditScoringEngine.Application.Exceptions;
using CreditScoringEngine.Application.Services;
using CreditScoringEngine.Domain.Entities;
using CreditScoringEngine.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CreditScoringEngine.Tests.Units;

public class PropostaServiceTests
{
    private readonly DbContextOptions<CreditScoringEngineContext> _options;

    public PropostaServiceTests()
    {
        _options = new DbContextOptionsBuilder<CreditScoringEngineContext>()
            .UseInMemoryDatabase(databaseName: "PropostaDbTest")
            .Options;
    }

    [Fact]
    public async Task AddProposta_DeveAdicionarComSucesso()
    {
        using var context = new CreditScoringEngineContext(_options);

        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            Nome = "Cliente Teste",
            Idade = 30,
            RendaMensal = 5000,
            HistoricoCreditoSimulado = "Normal"
        };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var proposta = new PropostaCredito
        {
            Id = Guid.NewGuid(),
            ValorSolicitado = 15000,
            DataProposta = DateTime.Now,
            Status = StatusProposta.Pendente,
            Justificativa = "Inicial",
            Cliente = cliente,
            ClienteId = cliente.Id
        };

        var service = new PropostaService(context);
        await service.AddAsync(proposta);

        var result = await service.GetAllAsync();
        Assert.Single(result);
    }

    [Fact]
    public async Task DeleteProposta_DeveRemoverComSucesso()
    {
        using var context = new CreditScoringEngineContext(_options);

        var cliente = new Cliente
        {
            Nome = "Deletável",
            Idade = 40,
            RendaMensal = 6000,
            HistoricoCreditoSimulado = "Excelente"
        };

        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var proposta = new PropostaCredito
        {
            Id = Guid.NewGuid(),
            ValorSolicitado = 5000,
            DataProposta = DateTime.Now,
            Status = StatusProposta.Pendente,
            Justificativa = "Teste",
            ClienteId = cliente.Id
        };
        context.PropostasCredito.Add(proposta);
        await context.SaveChangesAsync();

        var service = new PropostaService(context);
        await service.DeleteAsync(proposta);

        var ex = await Assert.ThrowsAsync<PropostaNotFoundExceptions>(async () =>
        {
            await service.GetByIdAsync(proposta.Id);
        });
        Assert.Contains("Proposta invalida, nao encontrado na base!", ex.Message);
    }
}
