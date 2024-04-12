using Application;
using Application.Ports;
using Data;
using Domain.Ports;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));


#region IOC
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientManager, ClientManager>();
#endregion

//ele documenta os enumns para string
var connectionString = builder.Configuration.GetConnectionString("Main");
builder.Services.AddDbContext<ClientDBContext>(
    options => options.UseSqlServer(connectionString)
);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
