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
                .ForMember(dto => dto.IsOwner, opt => {
                    opt.MapFrom(model => model.OrganizationOwned != null);
                })
                .ForMember(dto => dto.IsAdmin, opt => {
                    opt.MapFrom(model => model.TeamsAssigned.Any(t => !t.Team.IsCustom));
                })
                .ForMember(dto => dto.IsValid, opt => {
                    opt.MapFrom(model => DateTime.Now < model.Created.AddDays(7));
                });
        }
    }
}