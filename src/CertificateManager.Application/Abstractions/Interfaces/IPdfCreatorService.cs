using CertificateManager.Application.DataTransferObjects.UserDTOs;
using Microsoft.AspNetCore.Mvc;

namespace CertificateManager.Application.Abstractions.Interfaces;

public interface IPdfCreatorService
{
    public FileContentResult CreatePdf(List<UserUpdateDto> users);


}