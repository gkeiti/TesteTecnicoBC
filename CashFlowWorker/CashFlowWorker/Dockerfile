FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["/CashFlowWorker/CashFlowWorker/CashFlowWorker.csproj", "."]
RUN dotnet restore "./CashFlowWorker.csproj"

COPY . .
WORKDIR "/src/CashFlowWorker"

RUN dotnet build "./CashFlowWorker/CashFlowWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CashFlowWorker/CashFlowWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY wait-for-it.sh /usr/local/bin/wait-for-it.sh
CMD ["wait-for-it.sh", "rabbitmq:5672", "-t", "30", "--", "dotnet", "CashFlowWorker.dll"]