# .NET SDK kullanarak build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Proje dosyalarını kopyalayın
COPY ["Weatheryzer.API/Weatheryzer.API.csproj", "Weatheryzer.API/"]
COPY ["Weatheryzer.Application/Weatheryzer.Application.csproj", "Weatheryzer.Application/"]
COPY ["Weatheryzer.Domain/Weatheryzer.Domain.csproj", "Weatheryzer.Domain/"]
COPY ["Weatheryzer.Infrastructure/Weatheryzer.Infrastructure.csproj", "Weatheryzer.Infrastructure/"]
COPY ["Weatheryzer.Shared/Weatheryzer.Shared.csproj", "Weatheryzer.Shared/"]

# Restore işlemi
RUN dotnet restore "Weatheryzer.API/Weatheryzer.API.csproj"

# Kalan dosyaları kopyalayın
COPY . .

# Build işlemi
WORKDIR "/src/Weatheryzer.API"
RUN dotnet build "Weatheryzer.API.csproj" -c Release -o /app/build

# Publish işlemi
RUN dotnet publish "Weatheryzer.API.csproj" -c Release -o /app/publish

# Runtime için
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Weatheryzer.API.dll"]