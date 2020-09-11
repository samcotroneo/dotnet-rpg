using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Model;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper mapper;

        private readonly DataContext context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            // Map the AddCharacterDto to a standard Character.
            Character character = mapper.Map<Character>(newCharacter);

            // Add the new character to the data context.
            await context.AddAsync(character);

            // Save all data changes.
            await context.SaveChangesAsync();

            // Map the collection of database Characters and return service response.
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            serviceResponse.Data = context.Characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                Character dbCharacter = await context.Characters.FirstAsync(c => c.Id == id);
                context.Characters.Remove(dbCharacter);
                await context.SaveChangesAsync();

                serviceResponse.Data = context.Characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
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
            List<Character> dbCharacters = await context.Characters.ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            Character dbCharacter = await context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            serviceResponse.Data = mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            Character dbCharacter = await context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

            // This could also be a try/catch to catch other exceptions.
            if (dbCharacter != null)
            {
                // Update the character properties.
                dbCharacter.Name = updatedCharacter.Name;
                dbCharacter.HitPoints = updatedCharacter.HitPoints;
                dbCharacter.Strength = updatedCharacter.Strength;
                dbCharacter.Defense = updatedCharacter.Defense;
                dbCharacter.Intelligence = updatedCharacter.Intelligence;
                dbCharacter.Class = updatedCharacter.Class;

                context.Characters.Update(dbCharacter);
                await context.SaveChangesAsync();

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