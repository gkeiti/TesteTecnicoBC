# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5286


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src

COPY ["/CashFlowAPI/CashFlowAPI/CashFlowAPI.csproj", "CashFlowAPI/"]
RUN dotnet restore "./CashFlowAPI/CashFlowAPI.csproj"

COPY . .
WORKDIR "/src/CashFlowAPI"

RUN dotnet build "./CashFlowAPI/CashFlowAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./CashFlowAPI/CashFlowAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD ["dotnet", "CashFlowAPI.dll"]