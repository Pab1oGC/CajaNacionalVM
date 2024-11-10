# Imagen base para el entorno de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Imagen base para el entorno de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar el archivo del proyecto y restaurar dependencias
COPY ["CNSVM.csproj", "."]
RUN dotnet restore "./CNSVM.csproj"

# Copiar el resto de los archivos y construir la aplicación
COPY . .
RUN dotnet build "./CNSVM.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "./CNSVM.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagen final para el entorno de ejecución
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Development

# Establecer el punto de entrada
ENTRYPOINT ["dotnet", "CNSVM.dll"]
