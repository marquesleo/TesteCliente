using Application;
using Application.Ports;
using Data;
using Domain.Ports;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();
#endregion

//ele documenta os enumns para string
var connectionString = builder.Configuration.GetConnectionString("Main");
builder.Services.AddDbContext<ClientDBContext>(
    options => {
        options.UseSqlServer(connectionString);
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    
);
builder.Services.AddCors();
var key = Encoding.ASCII.GetBytes(Application.Services.Settings.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
 .AddJwtBearer(x =>
 {
     x.RequireHttpsMetadata = false;
     x.SaveToken = true;
     x.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(key),
         ValidateIssuer = false,
         ValidateAudience = false,
         ValidateLifetime = true,
         ClockSkew = TimeSpan.Zero,
     };
 })
 .AddCookie("Cookies", options =>
 {
     options.LoginPath = "/login";
     options.ExpireTimeSpan = TimeSpan.FromDays(7);
 });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
