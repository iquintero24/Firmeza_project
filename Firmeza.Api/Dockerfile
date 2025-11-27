# ----------------------------
#  BASE IMAGE
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# ----------------------------
#  BUILD IMAGE
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Firmeza.Api/Firmeza.Api.csproj", "Firmeza.Api/"]
COPY ["Firmeza.Application/Firmeza.Application.csproj", "Firmeza.Application/"]
COPY ["Firmeza.Domain/Firmeza.Domain.csproj", "Firmeza.Domain/"]
COPY ["Firmeza.Infrastructure/Firmeza.Infrastructure.csproj", "Firmeza.Infrastructure/"]

RUN dotnet restore "Firmeza.Api/Firmeza.Api.csproj"

COPY . .
WORKDIR "/src/Firmeza.Api"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ----------------------------
#  FINAL IMAGE
# ----------------------------
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Firmeza.Api.dll"]
