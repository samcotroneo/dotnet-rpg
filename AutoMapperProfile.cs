using AutoMapper;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Model;

namespace dotnet_rpg
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<UpdateCharacterDto, Character>();
            CreateMap<UpdateCharacterDto, GetCharacterDto>();
        }
    }
}