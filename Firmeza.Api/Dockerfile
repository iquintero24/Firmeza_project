# ----------------------------
# BASE IMAGE
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Escuchar en todas las interfaces
ENV ASPNETCORE_URLS=http://+:8080

# ----------------------------
# BUILD IMAGE
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar proyectos
COPY ["Firmeza.Api/Firmeza.Api.csproj", "Firmeza.Api/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "Firmeza.Api/Firmeza.Api.csproj"

# Copiar todo y publicar
COPY . .
WORKDIR "/src/Firmeza.Api"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ----------------------------
# FINAL IMAGE
# ----------------------------
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Ejecutar la API
ENTRYPOINT ["dotnet", "Firmeza.Api.dll"]
