FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/Services/Catalog/Cornerstone.Catalog.Api/Cornerstone.Catalog.Api.csproj", "Services/Catalog/Cornerstone.Catalog.Api/"]
COPY ["src/Services/Catalog/Cornerstone.Catalog.Application/Cornerstone.Catalog.Application.csproj", "Services/Catalog/Cornerstone.Catalog.Application/"]
COPY ["src/Services/Catalog/Cornerstone.Catalog.Domain/Cornerstone.Catalog.Domain.csproj", "Services/Catalog/Cornerstone.Catalog.Domain/"]
COPY ["src/Services/Catalog/Cornerstone.Catalog.Infrastructure/Cornerstone.Catalog.Infrastructure.csproj", "Services/Catalog/Cornerstone.Catalog.Infrastructure/"]
COPY ["src/BuildingBlocks/Cornerstone.Common/Cornerstone.Common.csproj", "BuildingBlocks/Cornerstone.Common/"]

RUN dotnet restore "Services/Catalog/Cornerstone.Catalog.Api/Cornerstone.Catalog.Api.csproj"

COPY src/ .
WORKDIR "/src/Services/Catalog/Cornerstone.Catalog.Api"
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Cornerstone.Catalog.Api.dll"]