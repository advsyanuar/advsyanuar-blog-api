FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /src

COPY ["portfolio-api.csproj", "./"]
RUN dotnet restore "portfolio-api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "portfolio-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "portfolio-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "portfolio-api.dll"]