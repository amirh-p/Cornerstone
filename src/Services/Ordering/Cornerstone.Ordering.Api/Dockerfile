FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Ordering/Cornerstone.Ordering.Api/Cornerstone.Ordering.Api.csproj", "src/Services/Ordering/Cornerstone.Ordering.Api/"]
COPY ["src/Services/Ordering/Cornerstone.Ordering.Infrastructure/Cornerstone.Ordering.Infrastructure.csproj", "src/Services/Ordering/Cornerstone.Ordering.Infrastructure/"]
COPY ["src/Services/Ordering/Cornerstone.Ordering.Application/Cornerstone.Ordering.Application.csproj", "src/Services/Ordering/Cornerstone.Ordering.Application/"]
COPY ["src/Services/Ordering/Cornerstone.Ordering.Domain/Cornerstone.Ordering.Domain.csproj", "src/Services/Ordering/Cornerstone.Ordering.Domain/"]
COPY ["src/Shared/Cornerstone.Common/Cornerstone.Common.csproj", "src/Shared/Cornerstone.Common/"]
RUN dotnet restore "./src/Services/Ordering/Cornerstone.Ordering.Api/Cornerstone.Ordering.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Ordering/Cornerstone.Ordering.Api"
RUN dotnet build "./Cornerstone.Ordering.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Cornerstone.Ordering.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cornerstone.Ordering.Api.dll"]