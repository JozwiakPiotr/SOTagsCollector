FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/SOTagsCollector.API/SOTagsCollector.API.csproj", "src/SOTagsCollector.API/"]
RUN dotnet restore "src/SOTagsCollector.API/SOTagsCollector.API.csproj"
COPY . .
WORKDIR "/src/src/SOTagsCollector.API"
RUN dotnet build "SOTagsCollector.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SOTagsCollector.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SOTagsCollector.API.dll"]
