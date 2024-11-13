FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY src/ .

WORKDIR /app/CashFlow.Api

# Identify dependencies and download all libraries needed
RUN dotnet restore

RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build-env /app/out .

# Tells the container to execute the following command
ENTRYPOINT ["dotnet", "CashFlow.Api.dll"]
