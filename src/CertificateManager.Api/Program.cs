using CertificateManager.Api.MiddleWares;
using CertificateManager.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCertificateManagerProjectServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// at startup, the application will automatically record the update database command
app.AutoMigrateAppDbContext();

// Create a user with the administrator role
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
