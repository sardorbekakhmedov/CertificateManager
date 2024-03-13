using CertificateManager.Api.SignalRHub;
using CertificateManager.Application.Abstractions.Interfaces;
using CertificateManager.Application.Abstractions.Interfaces.RepositoryServices;
using CertificateManager.Application.DataTransferObjects.UserDTOs;
using CertificateManager.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CertificateManager.Api.RabbitMQ;

public class PdfConsumer : IConsumer<UserUpdateListMessage>
{
    private readonly ICertificateService _certificateService;
    private readonly IPdfCreatorService _pdfCreatorService;
    private readonly IHubContext<CustomHub> _customHubContext;

    public PdfConsumer(
        ICertificateService certificateService,
        IPdfCreatorService pdfCreatorService,
        IHubContext<CustomHub> customHubContext)
    {
        _certificateService = certificateService;
        _pdfCreatorService = pdfCreatorService;
        _customHubContext = customHubContext;
    }

    public async Task Consume(ConsumeContext<UserUpdateListMessage> context)
    {
        var message = context.Message;

        if (message.Users is not null)
        {
            FileContentResult file = _pdfCreatorService.CreatePdf(message.Users);

            // Getting the contents of a file as an array of bytes
            byte[] fileBytes = file.FileContents;

            var certificate = new Certificate()
            {
                CertificateData = fileBytes
            };

            await _certificateService.CreateAsync(certificate);

            // Sending an array of bytes via SignalR
            var messageText = "You can download the ready-made certificate that you have created!";
            await _customHubContext.Clients.All.SendAsync("message", messageText);

            foreach (var user in message.Users)
            {
                Console.WriteLine(user.Username);
                Console.WriteLine(user.Age);
                Console.WriteLine(user.Email);
                Console.WriteLine(user.UserRole);
                Console.WriteLine("========================");
            }
        }
    }
}
