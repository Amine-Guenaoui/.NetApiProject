using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWALKS.Models.DTO;
using NZWALKS.Repository;

namespace NZWALKS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository,IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {
            var walkdifficultiesDomain = await walkDifficultyRepository.GetAllAsync();
            //convert 
            var walkdifficultiesDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkdifficultiesDomain);
            return Ok(walkdifficultiesDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]

        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);
            if (walkDifficulty == null)
            {
                return NotFound();
            }

            
            return Ok(mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty));

        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync( Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //convert to Domain 
            var walkDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
            };

            //call repo
            walkDomain = await walkDifficultyRepository.AddAsync(walkDomain);

            // Conert to DTO 
            var walkDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDomain);

            return CreatedAtAction(nameof(GetWalkDifficultyAsync) , new { id = walkDTO.Id } , walkDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {

            //convert DTO to domain 
            var walkDomain = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code,
             

            };

            walkDomain = await walkDifficultyRepository.UpdateAsync(id, walkDomain);
            if (walkDomain == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDomain);

            return Ok(walkDTO);


        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            var walk = await walkDifficultyRepository.DeleteAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Models.DTO.WalkDifficulty>(walk);

            return Ok(walkDTO);


        }

    }
}
