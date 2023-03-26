#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Server/CurrencyRateBattle_Server/CurrencyRateBattleServer.csproj", "src/Server/CurrencyRateBattle_Server/"]

RUN apt-get update

RUN dotnet restore "src/Server/CurrencyRateBattle_Server/CurrencyRateBattleServer.csproj"
COPY . .
WORKDIR "/src/src/Server/CurrencyRateBattle_Server"
RUN dotnet build "CurrencyRateBattleServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CurrencyRateBattleServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurrencyRateBattleServer.dll"]