
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5041


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["/BalanceAPI/BalanceAPI/BalanceAPI.csproj", "."]
RUN dotnet restore "./BalanceAPI.csproj"

COPY . .
WORKDIR "/src/BalanceAPI"

RUN dotnet build "./BalanceAPI/BalanceAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BalanceAPI/BalanceAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY wait-for-it.sh /usr/local/bin/wait-for-it.sh
CMD ["wait-for-it.sh", "rabbitmq:5672", "-t", "60", "--", "wait-for-it.sh", "sqlserver:1433", "-t", "60", "--", "dotnet", "BalanceAPI.dll"]

