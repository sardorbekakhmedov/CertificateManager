using CertificateManager.Api.MiddleWares;
using CertificateManager.Api.Extensions;
using CertificateManager.Api.SignalRHub;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogConfiguration();

builder.Services.AddCertificateManagerProjectServices(builder.Configuration);

builder.Services.AddCors();

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

/*app.UseCors(options =>
{
    options.WithOrigins("http://localhost:5078")
        .AllowAnyHeader()
        .AllowAnyMethod();
});*/


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCustomErrorHandlerMiddleware();
app.MapControllers();

app.MapHub<CustomHub>("/custom-hub ");

app.Run();
