FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CadCamMachining.Server/CadCamMachining.Server.csproj", "CadCamMachining.Server/"]
COPY ["CadCamMachining.Client/CadCamMachining.Client.csproj", "CadCamMachining.Client/"]
COPY ["CadCamMachining.Shared/CadCamMachining.Shared.csproj", "CadCamMachining.Shared/"]
RUN dotnet restore "CadCamMachining.Server/CadCamMachining.Server.csproj"
COPY . .
WORKDIR "/src/CadCamMachining.Server"
RUN dotnet build "CadCamMachining.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CadCamMachining.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CadCamMachining.Server.dll"]
