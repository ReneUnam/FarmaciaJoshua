using WebApi.Interface;
using WebApi.Implementation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IEstudianteService, EstudianteService>();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.Run();
