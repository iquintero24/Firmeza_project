FROM mcr.microsoft.com/dotnet/sdk:8.0 AS testrunner
WORKDIR /src

COPY ["Firmeza.Tests/Firmeza.Tests.csproj", "Firmeza.Tests/"]
COPY ["Firmeza.Api/Firmeza.Api.csproj", "Firmeza.Api/"]
COPY ["Firmeza.web/Firmeza.web.csproj", "Firmeza.web/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "Firmeza.Tests/Firmeza.Tests.csproj"

COPY . .
WORKDIR "/src/Firmeza.Tests"
CMD ["dotnet", "test", "--logger:trx"]
