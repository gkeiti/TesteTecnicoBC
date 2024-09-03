# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5041


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/BalanceAPI/BalanceAPI/BalanceAPI.csproj", "."]
RUN dotnet restore "./BalanceAPI.csproj"
COPY . .
WORKDIR "/src/BalanceAPI"
RUN dotnet build "./BalanceAPI/BalanceAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Install dotnet-ef tool
# RUN dotnet tool install --global dotnet-ef

# # Make sure the dotnet tools are in the PATH
# ENV PATH="$PATH:/root/.dotnet/tools"

# RUN dotnet tool list --global

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BalanceAPI/BalanceAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# RUN dotnet ef database update -p ./BalanceAPI/

COPY wait-for-it.sh /usr/local/bin/wait-for-it.sh
CMD ["wait-for-it.sh", "rabbitmq:5672", "-t", "60", "--", "wait-for-it.sh", "sqlserver:5672", "-t", "60", "--", "dotnet", "BalanceAPI.dll"]
# CMD ["wait-for-it.sh", "rabbitmq:5672", "-t", "60", "--", "wait-for-it.sh", "sqlserver:5672", "-t", "60", "--", "RUN", "dotnet", "ef", "database", "update", "-p", "./BalanceAPI/", "--", "dotnet", "CashFlowWorker.dll"]

# COPY wait-for-it.sh /usr/local/bin/wait-for-it.sh
# RUN chmod +x /usr/local/bin/wait-for-it.sh

# COPY entrypoint.sh /usr/local/bin/entrypoint.sh
# RUN chmod +x /usr/local/bin/entrypoint.sh

# ENTRYPOINT ["/usr/local/bin/entrypoint.sh"]

# ENTRYPOINT ["dotnet", "BalanceAPI.dll"]