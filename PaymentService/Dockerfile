# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar la solución y restaurar
COPY *.sln .
COPY Payment.Api/Payment.Api.csproj ./Payment.Api/
COPY Payment.Application/Payment.Application.csproj ./Payment.Application/
COPY Payment.Domain/Payment.Domain.csproj ./Payment.Domain/
COPY Payment.Infrastructure/Payment.Infrastructure.csproj ./Payment.Infrastructure/
COPY Contracts/Contracts.csproj ./Contracts/

RUN dotnet restore

# Copiar el resto del código
COPY . .

# Publicar la aplicación
WORKDIR /app/Payment.Api
RUN dotnet publish -c Release -o /app/publish

# Etapa final - runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Puerto expuesto (ajústalo si usas otro)
EXPOSE 8080

ENTRYPOINT ["dotnet", "Payment.Api.dll"]
