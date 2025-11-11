using Grpc.Net.Client;
using Tickets;

// Habilitar HTTP/2 inseguro (H2C)
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var grpcUrl = "http://api:7182";
var channel = GrpcChannel.ForAddress(grpcUrl);
var client = new TicketProtoService.TicketProtoServiceClient(channel);

Console.WriteLine($"[CLIENTE] Conectando a gRPC en {grpcUrl}...");

Console.WriteLine($"Conectando a gRPC en {grpcUrl}...");
await Task.Delay(10000);

try
{

    Console.WriteLine("\n Trayendo la Página 1, con 5 Registros por Página...");

    var listRequest = new ListarTicketsRequest
    {
        PageNumber = 1,
        PageSize = 5
    };

    var listResponse = await client.ListarTicketsAsync(listRequest);

    Console.WriteLine($"\n Total de Tickets Recibidos en la página: {listResponse.Tickets.Count}");

    Console.WriteLine("\n Agregando nuevo Ticket via gRPC...");
    var newTicketRequest = new AgregarTicketRequest { Usuario = "gRPC_Usuario" };
    var createResponse = await client.AgregarTicketAsync(newTicketRequest);
    Console.WriteLine($"\n Ticket Creado. ID: {createResponse.Id}, Usuario: {createResponse.Usuario}");
    var createdId = createResponse.Id;
    var initialStatus = createResponse.Estatus;

    Console.WriteLine($"\n Actualizando Ticket ID {createdId} de {initialStatus} a 'CERRADO'...");
    var updateRequest = new ActualizarTicketRequest
    {
        Id = createdId,
        Usuario = createResponse.Usuario + " (Actualizado)",
        Estatus = "ABIERTO"
    };
    var updateResponse = await client.ActualizarTicketAsync(updateRequest);
    Console.WriteLine($"\n Ticket Actualizado. Estatus Nuevo: {updateResponse.Estatus}");

    Console.WriteLine($"\n Obteniendo el Ticket ID {createdId} y verificando el estatus...");
    var getRequest = new ObtenerTicketRequest { Id = createdId };
    var getResponse = await client.ObtenerTicketAsync(getRequest);

    Console.WriteLine($"\n Ticket Encontrado. Nombre: {getResponse.Usuario} ,Estatus: {getResponse.Estatus}");

    Console.WriteLine($"\n Eliminando Lógicamente Ticket ID {createdId}...");
    var deleteRequest = new EliminarTicketRequest { Id = createdId };

    var deleteResponse = await client.EliminarTicketAsync(deleteRequest);

    if (deleteResponse.Success)
    {
        Console.WriteLine($"\n Registro ID {createdId} **ELIMINADO LÓGICAMENTE** de la base de datos.");
    }
    else
    {
        Console.WriteLine($"\n La operación de eliminación falló o el ticket no fue encontrado.");
    }
}
catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
{
    Console.WriteLine($"\n ERROR gRPC: No se encontró el recurso (HTTP Code: {ex.StatusCode}).");
}
catch (Exception ex)
{
    Console.WriteLine($"\n ERROR DE CONEXIÓN: Asegúrate de que la API esté corriendo en {channel} y el puerto sea correcto (usando HTTP/2).");
    Console.WriteLine($"\n Detalle: {ex.Message}");
}
finally
{
    Console.WriteLine("\n Pruebas gRPC terminadas.");
}