
# CreditScoringEngine

[![Build Status](https://github.com/JoaoSimino/CreditScoringEngine/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/JoaoSimino/CreditScoringEngine/actions/workflows/ci-cd.yml)
[![Tests](https://img.shields.io/badge/tests-passing-brightgreen.svg)]()

## Sumário

- [Descrição](#descrição)
- [Arquitetura e Tecnologias](#arquitetura-e-tecnologias)
- [Funcionalidades](#funcionalidades)
- [Pré-requisitos](#pré-requisitos)
- [Como rodar](#como-rodar)
- [Testes](#testes)
- [Documentação da API](#documentação-da-api)
- [Contribuindo](#contribuindo)
- [Pipeline CI/CD](#pipeline-cicd)
- [Licença](#licença)

## Descrição

O CreditScoringEngine é um microserviço responsável por processar propostas de crédito enviadas por clientes cadastrados.
Ao receber uma solicitação, o sistema realiza um cruzamento inteligente dos dados do cliente e do valor solicitado para calcular um Score Interno e gerar uma justificativa para a aprovação ou reprovação da proposta de crédito.
As propostas aprovadas são automaticamente encaminhadas para um sistema de billing via integração com RabbitMQ, garantindo comunicação assíncrona, segura e escalável entre os serviços.

## Arquitetura e Tecnologias
Este projeto aplica boas práticas de arquitetura limpa no .NET 8, com foco em testes automatizados, logging estruturado, containerização e CI/CD, proporcionando robustez e qualidade para ambientes financeiros.
- .NET 8 Minimal APIs
- Clean Architecture com separação em camadas:
  - **Domain**: entidades e regras de negócio
  - **Application**: serviços e regras de aplicação
  - **Infrastructure**: persistência (EF Core)
  - **API**: endpoints, configuração e bootstrap
- Entity Framework Core com InMemory para testes
- Logging com Serilog (console e MSSQL Server)
- xUnit para testes unitários e de integração
- GitHub Actions para CI/CD

## Funcionalidades

- Gerenciamento completo de Clientes: cadastro, consulta, atualização e exclusão (CRUD) via endpoints RESTful
- Gerenciamento de Propostas de Crédito: criação, consulta, atualização e exclusão, com cálculo automático do Score Interno e justificativa
- Processamento automático das propostas aprovadas e integração com sistema de billing via RabbitMQ
- Tratamento global e consistente de erros com middleware personalizado
- Logging estruturado e enriquecido para facilitar auditoria e monitoramento
- Cobertura de testes unitários e de integração para garantir estabilidade e qualidade do sistema

## Pré-requisitos

- .NET SDK 8.0 instalado (Download)
- SQL Server local ou via container Docker (exemplo da imagem oficial: mcr.microsoft.com/mssql/server:2022-latest)
- Visual Studio 2022 ou VS Code recomendado
- Docker instalado (opcional, para rodar container isolado do SQL Server ou da aplicação)

## Como rodar

Clone o repositório:

```bash
git clone https://github.com/JoaoSimino/CreditScoringEngine.git
cd CreditScoringEngine
```

Restaurar dependências e rodar a aplicação:

```bash
dotnet restore CreditScoringEngine.sln
dotnet run --project CreditScoringEngine.API
```

A API estará disponível em `http://localhost:5000` (ou porta configurada).

## Testes

Para rodar todos os testes:

```bash
dotnet test CreditScoringEngine.sln --configuration Release --verbosity normal
```

Os testes utilizam banco InMemory para isolamento total.

## Documentação da API

A API está configurada com Swagger UI para facilitar testes e visualização da documentação.  
Acesse `http://localhost:5000/swagger` após rodar a aplicação.

Exemplo de requisição curl para listar Clientes:

```bash
curl -X GET http://localhost:5000/api/Cliente
```

## Contribuindo

Contribuições são bem-vindas! Para contribuir:

1. Fork este repositório
2. Crie uma branch feature com o padrão `feature/nome-da-feature`
3. Faça commits claros e descritivos
4. Abra um Pull Request detalhando as alterações

Por favor, siga o padrão de commits [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/).

## Pipeline CI/CD

O projeto utiliza GitHub Actions para:

- Validar o código a cada push/PR na branch `main`
- Executar testes automaticamente
- Buildar e preparar o pacote para release, e subir ja um container atualizar para o Docker hub. 

O workflow está disponível em `.github/workflows/ci-cd.yml`.

## Licença

Este projeto está licenciado sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

Obrigado por usar o CreditScoringEngine!  
Para dúvidas ou sugestões, abra uma issue ou entre em contato comigo.
