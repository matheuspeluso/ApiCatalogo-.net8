using System.Text.Json.Serialization;
using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. metodo equivalente ao ConfigureServices() da classe startup

builder.Services.AddControllers().
        AddJsonOptions(options=> 
            options.JsonSerializerOptions.
                ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//obtendo a string de conexão:
string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
                        options.UseMySql(mySqlConnection, 
                        ServerVersion.AutoDetect(mySqlConnection)));

var app = builder.Build();

// Configure the HTTP request pipeline. equivalente ao metodo Configure() da classe startup
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); //incluir os endpoints do metodos actions do controlador

app.Run();
