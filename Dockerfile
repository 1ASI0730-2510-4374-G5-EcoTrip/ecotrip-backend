# Use the official .NET 9.0 runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 10000

# Use the official .NET 9.0 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["ecotrip-backend/ecotrip-backend.csproj", "ecotrip-backend/"]
RUN dotnet restore "ecotrip-backend/ecotrip-backend.csproj"
COPY . .
WORKDIR "/src/ecotrip-backend"
RUN dotnet build "ecotrip-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ecotrip-backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://*:10000
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "ecotrip-backend.dll"]
