# container de mssql para substituir o banco ate entao rodando local
#comando para rodar meu container passando a string de conexao correta!
#docker run --rm -e "ConnectionStrings__DefaultConnection=Server=host.docker.internal,1433;Database=CreditScoringEngine;User Id=sa;Password=MyStr0ng!Passw0rd;TrustServerCertificate=True;" -p 8080:80 minimalapikickoff
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=MyStr0ng!Passw0rd" -p 1433:1433 --name sqlserver-container -d mcr.microsoft.com/mssql/server:2022-latest