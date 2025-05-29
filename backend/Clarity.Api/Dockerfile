FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Clarity.Api/Clarity.Api.csproj", "Clarity.Api/"]
COPY ["Clarity.Core/Clarity.Core.csproj", "Clarity.Core/"]
COPY ["Clarity.Persistence/Clarity.Persistence.csproj", "Clarity.Persistence/"]
RUN dotnet restore "Clarity.Api/Clarity.Api.csproj"
COPY . .
WORKDIR "/src/Clarity.Api"
RUN dotnet build "Clarity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Clarity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Clarity.Api.dll"]
