FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["Backend/Wholesaler.Backend.Api/Wholesaler.Backend.Api.csproj", "Backend/Wholesaler.Backend.Api/"]
COPY ["Backend/Wholesaler.Backend.Domain/Wholesaler.Backend.Domain.csproj", "Backend/Wholesaler.Backend.Domain/"]
COPY ["Backend/Wholesaler.Backend.DataAccess/Wholesaler.Backend.DataAccess.csproj", "Backend/Wholesaler.Backend.DataAccess/"]

RUN dotnet restore "Backend/Wholesaler.Backend.Api/Wholesaler.Backend.Api.csproj"

COPY . .

WORKDIR "/src/Backend/Wholesaler.Backend.Api"

RUN dotnet build "Wholesaler.Backend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "Wholesaler.Backend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Wholesaler.Backend.Api.dll"]