FROM mcr.microsoft.com/dotnet/sdk:8.0 AS testrunner
WORKDIR /src

COPY ["Firmeza.Tests/Firmeza.Tests.csproj", "Firmeza.Tests/"]
COPY ["Firmeza.Api/Firmeza.Api.csproj", "Firmeza.Api/"]
COPY ["Firmeza.Web/Firmeza.Web.csproj", "Firmeza.Web/"]
COPY ["Firmeza.Application/Firmeza.Application.csproj", "Firmeza.Application/"]
COPY ["Firmeza.Infrastructure/Firmeza.Infrastructure.csproj", "Firmeza.Infrastructure/"]
COPY ["Firmeza.Domain/Firmeza.Domain.csproj", "Firmeza.Domain/"]

RUN dotnet restore "Firmeza.Tests/Firmeza.Tests.csproj"

COPY . .
WORKDIR "/src/Firmeza.Tests"

CMD ["dotnet", "test", "--logger:trx"]
