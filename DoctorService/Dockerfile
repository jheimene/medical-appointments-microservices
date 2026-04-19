# Etapa 1: Construcción (build)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar la solución y proyectos para restaurar
COPY ["*.sln", "./"]
COPY ["src/ProductService.Api/ProductService.Api.csproj", "src/ProductService.Api/"]
COPY ["src/ProductService.Application/ProductService.Application.csproj", "src/ProductService.Application/"]
COPY ["src/ProductService.Domain/ProductService.Domain.csproj", "src/ProductService.Domain/"]
COPY ["src/ProductService.Infrastructure/ProductService.Infrastructure.csproj", "src/ProductService.Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "src/ProductService.Api/ProductService.Api.csproj"

# Copiar el resto del código
COPY . .

# Publicar
WORKDIR "/src/src/ProductService.Api"
RUN dotnet build "ProductService.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

# Publicar la aplicación
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProductService.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false


# Etapa 2: final - runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

# Puerto expuesto (ajústalo si usas otro)
EXPOSE 8080

ENTRYPOINT ["dotnet", "ProductService.Api.dll"]
