# Базовий образ для запуску додатку
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Базовий образ для збірки додатку
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо файл проекту та відновлюємо залежності
COPY ["FastMarketBackEnd/FastMarketBackEnd.csproj", "FastMarketBackEnd/"]
RUN dotnet restore "FastMarketBackEnd/FastMarketBackEnd.csproj"

# Копіюємо всі файли та збираємо додаток
COPY . .
WORKDIR "/src/FastMarketBackEnd"
RUN dotnet build "FastMarketBackEnd.csproj" -c Release -o /app/build

# Публікуємо додаток
FROM build AS publish
RUN dotnet publish "FastMarketBackEnd.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Фінальний образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FastMarketBackEnd.dll"]

RUN apt-get update && \
    apt-get install -y wget && \
    wget https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh && \
    chmod +x dotnet-install.sh && \
    ./dotnet-install.sh --version 8.0.406
