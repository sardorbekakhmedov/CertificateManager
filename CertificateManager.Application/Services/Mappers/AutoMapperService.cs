using AutoMapper;
using Certificate.Application.DataTransferObjects.CertificateDTOs;
using Certificate.Application.DataTransferObjects.UserDTOs;
using CertificateManager.Domain.Entities;

namespace Certificate.Application.Services.Mappers;

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
        CreateMap<CertificateCreateDto, CertificateManager.Domain.Entities.Certificate>().ReverseMap();
        CreateMap<CertificateManager.Domain.Entities.Certificate, CertificateDto>().ReverseMap();
        #endregion
    }
}