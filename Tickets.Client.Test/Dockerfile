# Tickets.Client.Test/Dockerfile (VERSIÓN CORREGIDA Y ROBUSTA)
# -------------------------------------------------------------
# FASE 1: BUILD
# =================================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiar archivos de proyecto y solución
# Copia el archivo de solución (asumiendo que está en la raíz)
COPY DoubleVPartners_SlnTickets.sln .

# Copia el archivo .csproj del cliente a su subcarpeta de origen
COPY Tickets.Client.Test/Tickets.Client.Test.csproj Tickets.Client.Test/

# 2. Restaurar dependencias (esto usa la solución y el csproj)
RUN dotnet restore "Tickets.Client.Test/Tickets.Client.Test.csproj"

# 3. Copiar el resto del código fuente del proyecto
# Origen (en tu disco, desde la raíz): Tickets.Client.Test/
# Destino (en contenedor): Tickets.Client.Test/ (donde está el csproj)
COPY Tickets.Client.Test/ Tickets.Client.Test/

# 4. Publicar el proyecto en Release
# Navegamos al directorio del proyecto:
WORKDIR /src/Tickets.Client.Test 
# Y ejecutamos publish solo con el nombre del archivo, ya que estamos dentro de su carpeta.
RUN dotnet publish "Tickets.Client.Test.csproj" -c Release -o /app/publish /p:UseAppHost=false

# =================================================================
# FASE 2: RUNTIME
# =================================================================
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app

# Copiar la publicación desde la fase de build
COPY --from=build /app/publish .

# Ejecutable
ENTRYPOINT ["dotnet", "Tickets.Client.Test.dll"]