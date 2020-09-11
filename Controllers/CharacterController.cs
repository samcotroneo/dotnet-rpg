using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Model;
using dotnet_rpg.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService characterService;

        public CharacterController(ICharacterService characterService)
        {
            this.characterService = characterService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            return Ok(await characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            return Ok(await characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddCharacter(AddCharacterDto newCharacter)
        {
            return Ok(await characterService.AddCharacter(newCharacter));
        }


        [HttpPut]
        public async Task<IActionResult> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = await characterService.UpdateCharacter(updatedCharacter);

            // Can also null check Data instead of checking success.
            if (serviceResponse.Success)
            {
                return Ok(serviceResponse);
            }
            else
            {
                return NotFound(serviceResponse);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = await characterService.DeleteCharacter(id);

            // Can also null check Data instead of checking success.
            if (serviceResponse.Success)
            {
                return Ok(serviceResponse);
            }
            else
            {
                return NotFound(serviceResponse);
            }
        }
    }
}