#!/usr/bin/env bash
# entrypoint.sh

# Aguardar RabbitMQ e SQL Server
/usr/local/bin/wait-for-it.sh rabbitmq:5672 -t 60
/usr/local/bin/wait-for-it.sh sqlserver:1433 -t 60

dotnet tool install --global dotnet-ef
dotnet tool list --global

# Rodar EF migrations
dotnet ef database update --project /app/BalanceAPI.dll

# Iniciar o aplicativo
exec dotnet BalanceAPI.dll
