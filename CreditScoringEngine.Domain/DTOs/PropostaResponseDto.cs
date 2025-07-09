using CreditScoringEngine.Domain.Entities;

namespace CreditScoringEngine.Domain.DTOs;

public record PropostaResponseDto(
    Guid PropostaId,
    decimal ValorSolicitado,
    DateTime DataDeCriacao,
    StatusProposta Status,
    Guid ClienteId
);
