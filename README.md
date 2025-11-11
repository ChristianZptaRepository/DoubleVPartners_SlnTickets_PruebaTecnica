**Ejecución del Proyecto**

_Recomiendo abrir el archivo word o documentos de google compartido mediante el correo_

Abrir el Proyecto en Visual Studio 2022
Limpiar el Proyecto y Compilar
Ejecutar el Programa Docker
Tener el Motor de Bases de Datos SQl Server Instalado

****Comandos****

Realizando el comando ‘ctrl + ñ’ para que en el Visual Studio se abra una nueva consola de desarrolladores.

Ejecutar los siguientes comandos en el mismo orden:

1.  docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=SQL#123456789" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
2.  docker cp .\scripts\init.sql sqlserver:/init.sql
3. docker exec -it sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "SQL#123456789" -C -i /init.sql


Una vez realizado estos pasos, ya podemos ejecutar el siguiente comando:

5.  docker compose up --build

Y ya para acceder a las URL’s lo haremos de la siguiente manera

**REST Swagger**

Esta es la URL de Ingreso: http://localhost:7181/swagger/index.html

**GraphQL**

Esta es la URL de ingreso: http://localhost:7181/graphql/

Queries y Mutations Para GraphQL

**Obtener todo la lista de registros**

query {
  obtenerTickets(page: 1, pageSize: 5) {
    items {
    id
    usuario
    fechaCreacion
    estatus
    eliminado
    }
  }
}

**Obtener un registro por id**

query {
  obtenerTicketsPorId(id: 1) {
    id
    usuario
    fechaCreacion
    estatus
    eliminado
  }
}

**Agregar un Ticket**

mutation {
  agregarTicket(input: { usuario: "UsuarioTest" }) {
    id
    usuario
    fechaCreacion
    estatus
    eliminado
  }
}

**Actualizar un Ticket**

mutation {
  actualizarTicket(id: 1, input: { usuario: "UsuarioTest", estatus: CERRADO }) {
    id
    usuario
    fechaCreacion
    estatus
    eliminado
  }
}

**Eliminado Logico**

mutation {
  eliminarTicket(id: 6) {
    
  }
}


