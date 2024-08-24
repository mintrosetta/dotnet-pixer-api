using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PixerAPI.Contexts;
using PixerAPI.UnitOfWorks;
using PixerAPI.UnitOfWorks.Interfaces;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IRepoUnitOfWork, RepoUnitOfWork>();
builder.Services.AddScoped<IServiceUnitOfWork, ServiceUnitOfWork>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<MySQLDbContext>(option => {
    string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (connectionString == null) throw new Exception("Connection string not found");

    option.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mysqlOption => mysqlOption.SchemaBehavior(MySqlSchemaBehavior.Ignore));
});
builder.Services.AddAuthentication(authOption =>
{
    authOption.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOption.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>(),
        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Get<string>(),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Get<string>()!))
    };
});
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();