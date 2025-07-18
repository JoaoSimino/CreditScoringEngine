﻿using CreditScoringEngine.Domain.DTOs;

namespace CreditScoringEngine.Domain.Entities;

public static class PropostaExtensions
{
    public static PropostaCredito DtoToEntity(this PropostaCredito proposta, PropostaDto propostadto, Cliente cliente)
    {
        proposta.Id = Guid.NewGuid();
        proposta.ValorSolicitado = propostadto.ValorSolicitado;
        proposta.DataProposta = DateTime.Now;
        proposta.Status = StatusProposta.Pendente;
        proposta.Score = new ScoreInterno
        {
            Faixa = FaixaRisco.Baixo,
            Valor = 0
        };
        proposta.Justificativa = "em analise....";
        proposta.Cliente = cliente;

        return proposta;
    }
    public static PropostaCredito Update(this PropostaCredito proposta, PropostaDto propostadto, Cliente cliente)
    {
        proposta.ValorSolicitado = propostadto.ValorSolicitado;
        proposta.DataProposta = DateTime.Now;
        proposta.Status = StatusProposta.Pendente;
        proposta.Score = new ScoreInterno
        {
            Faixa = FaixaRisco.Baixo,
            Valor = 0
        };
        proposta.Justificativa = "em analise....";
        proposta.Cliente = cliente;

        return proposta;
    }
}
