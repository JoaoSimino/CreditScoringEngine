namespace CreditScoringEngine.Domain.DTOs;

public record ClienteDto(string Nome, decimal RendaMensal, int Idade, string HistoricoCreditoSimulado);
