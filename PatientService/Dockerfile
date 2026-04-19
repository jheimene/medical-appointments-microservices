# Etapa 1: Construcción (build)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar la solución y proyectos para restaurar
COPY ["*.sln", "./"]
COPY ["src/CustomerService.Api/CustomerService.Api.csproj", "src/CustomerService.Api/"]
COPY ["src/CustomerService.Application/CustomerService.Application.csproj", "src/CustomerService.Application/"]
COPY ["src/CustomerService.Domain/CustomerService.Domain.csproj", "src/CustomerService.Domain/"]
COPY ["src/CustomerService.Infrastructure/CustomerService.Infrastructure.csproj", "src/CustomerService.Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "src/CustomerService.Api/CustomerService.Api.csproj"

# Copiar el resto del código
COPY . .

# Publicar
WORKDIR "/src/src/CustomerService.Api"
RUN dotnet build "CustomerService.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

# Publicar la aplicación
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CustomerService.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false


# Etapa 2: final - runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

# Puerto expuesto (ajústalo si usas otro)
EXPOSE 8080

ENTRYPOINT ["dotnet", "CustomerService.Api.dll"]
