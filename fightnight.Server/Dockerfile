#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Need a base image which has basic instructions of how a dotnet App should be run
# In a container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080 
EXPOSE 8081 

# Building release version of app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["fightnight.client/nuget.config", "fightnight.client/"]
COPY ["fightnight.Server/fightnight.Server.csproj", "fightnight.Server/"]
COPY ["fightnight.client/fightnight.client.esproj", "fightnight.client/"]
# Restoring all nuget packages
RUN dotnet restore "./fightnight.Server/fightnight.Server.csproj"
COPY . .
WORKDIR "/src/fightnight.Server"
RUN dotnet build "./fightnight.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish App
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./fightnight.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Run App
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "fightnight.Server.dll"]