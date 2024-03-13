using CertificateManager.Application.Abstractions.Interfaces;
using CertificateManager.Application.Abstractions.Interfaces.RepositoryServices;
using CertificateManager.Application.DataTransferObjects.UserDTOs;
using CertificateManager.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace CertificateManager.Api.RabbitMQ;

public class PdfConsumer : IConsumer<UserUpdateListMessage>
{
    private readonly ICertificateService _certificateService;
    private readonly IPdfCreatorService _pdfCreatorService;

    public PdfConsumer(
        ICertificateService certificateService,
        IPdfCreatorService pdfCreatorService)
    {
        _certificateService = certificateService;
        _pdfCreatorService = pdfCreatorService;
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
