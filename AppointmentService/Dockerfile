# Etapa 1: Construcción (build)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /workspace

# Copiar la solución y proyectos para aprovechar el cache de Docker
COPY ["OrderService/OrderService.sln", "OrderService/"]
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
COPY ["OrderService/src/OrderService.Api/OrderService.Api.csproj", "OrderService/src/OrderService.Api/"]
COPY ["OrderService/src/OrderService.Application/OrderService.Application.csproj", "OrderService/src/OrderService.Application/"]
COPY ["OrderService/src/OrderService.Domain/OrderService.Domain.csproj", "OrderService/src/OrderService.Domain/"]
COPY ["OrderService/src/OrderService.Infrastructure/OrderService.Infrastructure.csproj", "OrderService/src/OrderService.Infrastructure/"]
COPY ["OrderService/src/OrderService.Contracts/OrderService.Contracts.csproj", "OrderService/src/OrderService.Contracts/"]

# Restaurar dependencias
RUN dotnet restore "OrderService/src/OrderService.Api/OrderService.Api.csproj"

# Copiar el resto del código
COPY BuildingBlocks ./BuildingBlocks
COPY OrderService ./OrderService

# Publicar
WORKDIR "/workspace/OrderService/src/OrderService.Api"
RUN dotnet build "OrderService.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/build --no-restore

# Publicar la aplicación
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /workspace/OrderService/src/OrderService.Api
RUN dotnet publish "OrderService.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false --no-restore


# Etapa 2: final - runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
ARG APP_PORT=8080
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:${APP_PORT}

COPY --from=publish /app/publish .

# Puerto expuesto (ajústalo si usas otro)
EXPOSE ${APP_PORT}

ENTRYPOINT ["dotnet", "OrderService.Api.dll"]
