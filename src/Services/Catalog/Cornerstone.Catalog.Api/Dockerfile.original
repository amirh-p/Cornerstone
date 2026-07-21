FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Catalog/Cornerstone.Catalog.Api/Cornerstone.Catalog.Api.csproj", "src/Services/Catalog/Cornerstone.Catalog.Api/"]
COPY ["src/Services/Catalog/Cornerstone.Catalog.Infrastructure/Cornerstone.Catalog.Infrastructure.csproj", "src/Services/Catalog/Cornerstone.Catalog.Infrastructure/"]
COPY ["src/Services/Catalog/Cornerstone.Catalog.Application/Cornerstone.Catalog.Application.csproj", "src/Services/Catalog/Cornerstone.Catalog.Application/"]
COPY ["src/Services/Catalog/Cornerstone.Catalog.Domain/Cornerstone.Catalog.Domain.csproj", "src/Services/Catalog/Cornerstone.Catalog.Domain/"]
COPY ["src/Shared/Cornerstone.Common/Cornerstone.Common.csproj", "src/Shared/Cornerstone.Common/"]
RUN dotnet restore "./src/Services/Catalog/Cornerstone.Catalog.Api/Cornerstone.Catalog.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Catalog/Cornerstone.Catalog.Api"
RUN dotnet build "./Cornerstone.Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Cornerstone.Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cornerstone.Catalog.Api.dll"]