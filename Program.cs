using FARMACIA_JOSHUA_RESTFUL.Services.Implementation;
using FARMACIA_JOSHUA_RESTFUL.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
var app = builder.Build();

app.UseRouting();

app.MapControllers();
app.UseHttpsRedirection();
app.Run();
