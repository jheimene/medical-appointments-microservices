FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/DoctorService.Api/DoctorService.Api.csproj", "src/DoctorService.Api/"]
COPY ["src/DoctorService.Application/DoctorService.Application.csproj", "src/DoctorService.Application/"]
COPY ["src/DoctorService.Domain/DoctorService.Domain.csproj", "src/DoctorService.Domain/"]
COPY ["src/DoctorService.Infrastructure/DoctorService.Infrastructure.csproj", "src/DoctorService.Infrastructure/"]

RUN dotnet restore "src/DoctorService.Api/DoctorService.Api.csproj"

COPY . .

WORKDIR "/src/src/DoctorService.Api"
RUN dotnet build "DoctorService.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DoctorService.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "DoctorService.Api.dll"]