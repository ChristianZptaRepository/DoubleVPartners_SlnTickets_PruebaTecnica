using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Tickets.API;
using Tickets.API.GraphQL;
using Tickets.Application.Repositorio.Interface;
using Tickets.Application.Tickets.ObtenerTicketPorId;
using Tickets.Infrastructure.Persistencia;
using Tickets.Infrastructure.TicketRepositorio;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ITicketRepositorio, TicketRepositorio>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ObtenerTicketPorIdQuery).Assembly));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
builder.Services
    .AddGraphQLServer()
    .AddQueryType<TicketQueries>()
    .AddMutationType<TicketMutations>();

//builder.WebHost.ConfigureKestrel(options =>
//{
//    //Configuración Para Docker Disponibles REST y GraphQL
//    options.ListenAnyIP(7181, listenOptions =>
//    {
//        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
//    });
//    //Configuracion Local Para Funcionamiento Conjuntode REST - GraphQL y gRPC
//    //options.ListenLocalhost(7181, listenOptions =>
//    //{
//    //    listenOptions.UseHttps();
//    //    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
//    //});
//});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tickets API V1");
        c.RoutePrefix = "swagger"; // URL: https://localhost:7181/swagger
    });
}
//DESCOMENTAR --- Configuracion Local Para Funcionamiento Conjuntode REST - GraphQL y gRPC
//app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<TicketGrpcImplService>();
app.MapGraphQL("/graphql");

app.Run();
