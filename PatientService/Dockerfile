FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/PatientService.Api/PatientService.Api.csproj", "src/PatientService.Api/"]
COPY ["src/PatientService.Application/PatientService.Application.csproj", "src/PatientService.Application/"]
COPY ["src/PatientService.Domain/PatientService.Domain.csproj", "src/PatientService.Domain/"]
COPY ["src/PatientService.Infrastructure/PatientService.Infrastructure.csproj", "src/PatientService.Infrastructure/"]

RUN dotnet restore "src/PatientService.Api/PatientService.Api.csproj"

COPY . .

WORKDIR "/src/src/PatientService.Api"
RUN dotnet build "PatientService.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PatientService.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "PatientService.Api.dll"]