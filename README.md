
# MinimalAPIKickoff

[![Build Status](https://github.com/JoaoSimino/MinimalAPIKickoff/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/JoaoSimino/MinimalAPIKickoff/actions/workflows/ci-cd.yml)
[![Tests](https://img.shields.io/badge/tests-passing-brightgreen.svg)]()
[![Coverage](https://img.shields.io/badge/coverage-unknown-lightgrey.svg)]()

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

MinimalAPIKickoff é um projeto de exemplo para demonstrar o uso de Minimal APIs com .NET 8 seguindo princípios de Clean Architecture.  
Ele contém uma API para gerenciamento básico de usuários, implementando boas práticas como injeção de dependência, logging estruturado com Serilog, testes automatizados e pipeline CI/CD.

Este projeto é ideal para quem deseja iniciar um template moderno para APIs .NET minimalistas, escaláveis e testáveis.

## Arquitetura e Tecnologias

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

- CRUD básico para usuários via endpoints RESTful
- Tratamento global de erros via middleware personalizado
- Logging estruturado e enriquecido
- Testes unitários e integração para garantir estabilidade

## Pré-requisitos

- .NET SDK 8.0 instalado ([Download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))
- SQL Server local (opcional para logs)
- Visual Studio 2022 ou VS Code recomendado
- Docker (opcional para ambiente isolado)

## Como rodar

Clone o repositório:

```bash
git clone https://github.com/JoaoSimino/MinimalAPIKickoff.git
cd MinimalAPIKickoff
```

Restaurar dependências e rodar a aplicação:

```bash
dotnet restore MinimalAPIKickoff.sln
dotnet run --project MinimalAPIKickoff.API
```

A API estará disponível em `http://localhost:5000` (ou porta configurada).

## Testes

Para rodar todos os testes:

```bash
dotnet test MinimalAPIKickoff.sln --configuration Release --verbosity normal
```

Os testes utilizam banco InMemory para isolamento total.

## Documentação da API

A API está configurada com Swagger UI para facilitar testes e visualização da documentação.  
Acesse `http://localhost:5000/swagger` após rodar a aplicação.

Exemplo de requisição curl para listar usuários:

```bash
curl -X GET http://localhost:5000/api/User
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
- Buildar e preparar o pacote para release

O workflow está disponível em `.github/workflows/ci-cd.yml`.

## Licença

Este projeto está licenciado sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

Obrigado por usar o MinimalAPIKickoff!  
Para dúvidas ou sugestões, abra uma issue ou entre em contato comigo.
