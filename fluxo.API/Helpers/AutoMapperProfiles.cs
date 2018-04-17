using System;
using System.Linq;
using AutoMapper;
using fluxo.API.DTO;
using fluxo.DATA.Models;

namespace fluxo.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserToRegisterDTO, User>();
            CreateMap<User, UserToListDTO>()
                .ForMember(dto => dto.IsOwner, opt => { opt.MapFrom(model => model.IsOwner()); })
                .ForMember(dto => dto.IsAdmin, opt => { opt.MapFrom(model => model.IsAdmin()); })
                .ForMember(dto => dto.IsValid, opt => { opt.MapFrom(model => model.IsValid()); });
            CreateMap<UserToEditDTO, User>();
            CreateMap<User, UserToEditDTO>()
                .ForMember(dto => dto.TeamIds, opt => { opt.MapFrom(model => model.TeamsAssigned.Select(ta => ta.TeamId).ToArray()); });
        }
    }
}