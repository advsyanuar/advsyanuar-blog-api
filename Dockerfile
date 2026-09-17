FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /src

COPY ["portfolio-api.csproj", "./"]
COPY ["Directory.Packages.props", "./"]
RUN dotnet restore "portfolio-api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "portfolio-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "portfolio-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/data /app/wwwroot/storage && chown -R app:app /app/data /app/wwwroot/storage

# DB file will be created here at runtime by EF Core migrations
VOLUME /app/data
# Uploaded images/videos live here and must persist across container restarts
VOLUME /app/wwwroot/storage

ENTRYPOINT ["dotnet", "portfolio-api.dll"]