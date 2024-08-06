﻿using AutoMapper;
using Entities.DTOs;
using Entities.Enums;
using Entities.Models;
using Utilities;

namespace Geared_Finance_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {


            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                .ForMember(dest => dest.VendorManagerLevelId, opt => opt.MapFrom(src => src.ManagerId))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password != null ? SecretHasher.DecryptString(src.Password) : null))
                .ReverseMap()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => (int)Enum.Parse(typeof(RoleEnum), src.RoleName.Replace(" ", ""))))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password != null ? SecretHasher.EnryptString(src.Password) : null))
                .ForMember(dest => dest.StaffCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.StaffCode) ? StringGenerator.GenerateUniqueString(3) : src.StaffCode))
                .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => Enum.Parse(typeof(RoleEnum), src.RoleName.Replace(" ", "")).ToString() == RoleEnum.VendorManager.ToString() ? src.VendorManagerLevelId : null))
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<Vendor, VendorDTO>();
            CreateMap<User, RelationshipManagerDTO>().ReverseMap();
            CreateMap<ManagerLevel, ManagerLevelDTO>().ReverseMap();
            CreateMap<FundingCategory, FundingCategoryDTO>().ReverseMap();
            CreateMap<FundingEquipmentType, EquipmentTypeDTO>().ReverseMap();
            CreateMap<FundingEquipmentType, EquipmentRepsonseDTO>().ReverseMap();
        }
    }

}
