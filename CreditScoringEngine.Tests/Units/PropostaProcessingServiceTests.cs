using CreditScoringEngine.Application.Services;
using CreditScoringEngine.Domain.Entities;
using Moq;

namespace CreditScoringEngine.Tests.Units;

public class PropostaProcessingServiceTests
{
    [Fact]
    public async Task Proposta_Com_Score_Alto_Deve_Ser_Aprovada_Com_Baixo_Risco()
    {
        // Arrange
        var proposta = new PropostaCredito
        {
            Id = Guid.NewGuid(),
            ValorSolicitado = 10000,
            Cliente = new Cliente
            {
                Nome = "João",
                RendaMensal = 12000,
                Idade = 40,
                HistoricoCreditoSimulado = "sem inadimplência nos últimos 12 meses"
            },
            Status = StatusProposta.Pendente,
            Score = new ScoreInterno()
        };

        var propostas = new List<PropostaCredito> { proposta };

        var mockService = new Mock<IPropostaService>();
        mockService.Setup(s => s.GetPropostaByStatusAsync(StatusProposta.Pendente))
                   .ReturnsAsync(propostas);
        mockService.Setup(s => s.UpdateAsync(It.IsAny<PropostaCredito>()))
                   .Returns(Task.CompletedTask);

        var processor = new PropostaProcessingService(mockService.Object);

        // Act
        await processor.ProcessarPropostasPendentesAsync();

        // Assert
        Assert.Equal(StatusProposta.Aprovada, proposta.Status);
        Assert.Equal(FaixaRisco.Baixo, proposta.Score.Faixa);
        Assert.Equal("Score elevado e baixo risco.", proposta.Justificativa);
        Assert.InRange(proposta.Score.Valor, 80, 100);
    }

    [Fact]
    public async Task Proposta_Com_Score_Medio_Deve_Ser_Aprovada_Com_Justificativa_Ressalva()
    {
        var proposta = new PropostaCredito
        {
            Id = Guid.NewGuid(),
            ValorSolicitado = 5000,
            Cliente = new Cliente
            {
                Nome = "Maria",
                RendaMensal = 6000,
                Idade = 21,
                HistoricoCreditoSimulado = "sem inadimplência nos últimos 12 meses"
            },
            Status = StatusProposta.Pendente,
            Score = new ScoreInterno()
        };

        var propostas = new List<PropostaCredito> { proposta };

        var mockService = new Mock<IPropostaService>();
        mockService.Setup(s => s.GetPropostaByStatusAsync(StatusProposta.Pendente))
                   .ReturnsAsync(propostas);
        mockService.Setup(s => s.UpdateAsync(It.IsAny<PropostaCredito>()))
                   .Returns(Task.CompletedTask);

        var processor = new PropostaProcessingService(mockService.Object);

        await processor.ProcessarPropostasPendentesAsync();

        Assert.Equal(StatusProposta.Aprovada, proposta.Status);
        Assert.Equal(FaixaRisco.Medio, proposta.Score.Faixa);
        Assert.Equal("Risco moderado. Proposta aprovada com ressalvas.", proposta.Justificativa);
    }

    [Fact]
    public async Task Proposta_Com_Score_Baixo_Deve_Ser_Recusada()
    {
        var proposta = new PropostaCredito
        {
            Id = Guid.NewGuid(),
            ValorSolicitado = 3000,
            Cliente = new Cliente
            {
                Nome = "Carlos",
                RendaMensal = 2000,
                Idade = 18,
                HistoricoCreditoSimulado = "inadimplente"
            },
            Status = StatusProposta.Pendente,
            Score = new ScoreInterno()
        };

        var propostas = new List<PropostaCredito> { proposta };

        var mockService = new Mock<IPropostaService>();
        mockService.Setup(s => s.GetPropostaByStatusAsync(StatusProposta.Pendente))
                   .ReturnsAsync(propostas);
        mockService.Setup(s => s.UpdateAsync(It.IsAny<PropostaCredito>()))
                   .Returns(Task.CompletedTask);

        var processor = new PropostaProcessingService(mockService.Object);

        await processor.ProcessarPropostasPendentesAsync();

        Assert.Equal(StatusProposta.Recusada, proposta.Status);
        Assert.Equal(FaixaRisco.Alto, proposta.Score.Faixa);
        Assert.Equal("Alto risco de inadimplência.", proposta.Justificativa);
    }
}
