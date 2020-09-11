using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Model;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>
        {
            new Character(),
            new Character()
            {
                Id = 1,
                Name = "Sam"
            }
        };

        private readonly IMapper mapper;

        public CharacterService(IMapper mapper)
        {
            this.mapper = mapper;

        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            // Map the AddCharacterDto to a standard Character, then add it.
            Character character = mapper.Map<Character>(newCharacter);
            character.Id = characters.Max(c => c.Id + 1);
            characters.Add(character);

            // Map the collection of Characters and return service response.
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            serviceResponse.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                Character character = characters.First(c => c.Id == id);
                characters.Remove(character);
                
                serviceResponse.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
                serviceResponse.Success = true;
                serviceResponse.Message = $"Character (Id: {id}) removed successfull.";

            }
            catch
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Character update failed, character with Id: {id} not found.";

            }

            return serviceResponse;

        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            serviceResponse.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            serviceResponse.Data = mapper.Map<GetCharacterDto>(characters.FirstOrDefault(c => c.Id == id));
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            Character character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);

            // This could also be a try/catch to catch other exceptions.
            if (character != null)
            {
                // Update the character properties.
                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strength = updatedCharacter.Strength;
                character.Defense = updatedCharacter.Defense;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = updatedCharacter.Class;

                serviceResponse.Data = mapper.Map<GetCharacterDto>(updatedCharacter);
                serviceResponse.Success = true;
                serviceResponse.Message = $"Character (Id: {updatedCharacter.Id}) update successfull.";
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Character update failed, character with Id: {updatedCharacter.Id} not found.";
            }

            return serviceResponse;
        }
    }
}