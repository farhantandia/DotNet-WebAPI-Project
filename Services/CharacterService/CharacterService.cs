using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        // private static List<Character> characters = new List<Character> {
        //     new Character(),
        //     new Character {Id=1, Name = "Ruby" }
        // };
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public CharacterService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;

        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
            // from local code
            // character.Id = characters.Max(c => c.Id) + 1;
            // characters.Add(character);
            // serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

            //from DB
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data =await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }



        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter(int userId)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.Where(c=>c.User!.Id == userId).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {

            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dBCharacter = _context.Characters.FirstOrDefault(c => c.Id == id); 
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dBCharacter);
            return serviceResponse;


        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {

                // var character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id); //characters from code, _context.Characters from DB
                
                var character = await _context.Characters.FirstOrDefaultAsync(c=>c.Id==updatedCharacter.Id);
                if(character is null){
                    throw new Exception($"Character with Id '{updatedCharacter.Id} not found");
                }
                _mapper.Map(updatedCharacter, character);
                // character.Name = updatedCharacter.Name;
                // character.HitPoints = updatedCharacter.HitPoints;
                // character.Strength = updatedCharacter.Strength;
                // character.Intelligence = updatedCharacter.Intelligence;
                // character.Class = updatedCharacter.Class;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;

            }
            return serviceResponse;
        }
         public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {

                // var character = characters.FirstOrDefault(c => c.Id ==id);
                var character =await  _context.Characters.FirstOrDefaultAsync(c=>c.Id == id);
                if(character is null){
                    throw new Exception($"Character with Id '{id} not found");
                }
                // characters.Remove(character);
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                // serviceResponse.Data = characters.Select(c=> _mapper.Map<GetCharacterDto>(c)).ToList();
                serviceResponse.Data =await _context.Characters.Select(c=> _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;

            }
            return serviceResponse;
        }

    }
}