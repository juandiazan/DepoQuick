FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["InterfazDeUsuario/InterfazDeUsuario.csproj", "InterfazDeUsuario/"]
COPY ["Repositorio/Repositorio.csproj", "Repositorio/"]
COPY ["Dominio/Dominio.csproj", "Dominio/"]
COPY ["LogicaNegocio/LogicaNegocio.csproj", "LogicaNegocio/"]
RUN dotnet restore "InterfazDeUsuario/InterfazDeUsuario.csproj"
COPY . .
WORKDIR "/src/InterfazDeUsuario"
RUN dotnet build "InterfazDeUsuario.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "InterfazDeUsuario.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InterfazDeUsuario.dll"]
