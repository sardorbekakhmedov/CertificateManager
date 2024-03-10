using AutoMapper;
using CertificateManager.Application.DataTransferObjects.CertificateDTOs;
using CertificateManager.Application.DataTransferObjects.UserDTOs;
using CertificateManager.Domain.Entities;

namespace CertificateManager.Application.Services.Mappers;

public class AutoMapperService : Profile
{
    public AutoMapperService()
    {
        #region User
        CreateMap<UserCreateDto, User>().ReverseMap();
        CreateMap<UserUpdateDto, User>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        #endregion


        #region Certificate
        CreateMap<CertificateCreateDto, Domain.Entities.Certificate>().ReverseMap();
        CreateMap<Domain.Entities.Certificate, CertificateDto>().ReverseMap();
        #endregion
    }
}