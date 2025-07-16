FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln ./
COPY CreditScoringEngine.API/*.csproj ./CreditScoringEngine.API/
COPY CreditScoringEngine.Tests/*.csproj ./CreditScoringEngine.Tests/
COPY CreditScoringEngine.Domain/*.csproj ./CreditScoringEngine.Domain/
COPY CreditScoringEngine.Application/*.csproj ./CreditScoringEngine.Application/
COPY CreditScoringEngine.Infrastructure/*.csproj ./CreditScoringEngine.Infrastructure/
RUN dotnet restore

COPY . .
RUN dotnet publish CreditScoringEngine.API/CreditScoringEngine.API.csproj -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "CreditScoringEngine.API.dll"]