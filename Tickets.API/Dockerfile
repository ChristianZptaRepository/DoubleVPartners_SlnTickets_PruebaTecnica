# =================================================================
# FASE 1: BUILD
# =================================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# **** CAMBIO CRUCIAL: Reemplazamos todas las instrucciones COPY anteriores ****
# Copiamos la carpeta 'src' entera y el archivo de solución (asumido en la raíz)
# Asume que tu archivo de solución se llama "DoubleVPartners_SlnTickets.sln"
COPY ["DoubleVPartners_SlnTickets.sln", "."]

COPY .	.
# *************************************************************************

# Ejecutamos la restauración sobre el archivo de solución
RUN dotnet restore "DoubleVPartners_SlnTickets.sln"

# Publica la aplicación final
WORKDIR "/src/Tickets.API"
RUN dotnet publish "Tickets.API.csproj" -c Release -o /app/publish /p:UseAppHost=false
# =================================================================
# FASE 2: FINAL (Runtime)
# =================================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
# Copia los archivos publicados desde la fase 'build'
COPY --from=build /app/publish .

# Exponer puertos
EXPOSE 7181
EXPOSE 7182

# Comando para iniciar la aplicación (Usando el nombre de tu DLL)
ENTRYPOINT ["dotnet", "Tickets.API.dll"]