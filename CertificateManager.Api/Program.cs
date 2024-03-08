using Certificate.Application.Abstractions.Interfaces;
using Certificate.Infrastructure.Persistence;
using CertificateManager.Api.MiddleWares;
using CertificateManager.Api.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCertificateApiServices(builder.Configuration);

/*AppContext.SetSwitch(switchName: "Npgsql.EnableLegacyTimestampBehavior", isEnabled: true);
builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
{
    options.UseInMemoryDatabase("UseInMemoryDatabase");
    //.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
});*/

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// at startup, the application will automatically record the update database command
app.AutoMigrateAppDbContext();

// Create admin
app.CreateDefaultUserSeedData();

app.UseCors(option =>
    option.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCustomErrorHandlerMiddleware();
app.MapControllers();

app.Run();
