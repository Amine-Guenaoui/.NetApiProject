using AutoMapper;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using NZWALKS.Models.DTO;
using NZWALKS.Repository;
using System.Reflection.Metadata.Ecma335;

namespace NZWALKS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //fetch data
            var walksdomain = await walkRepository.GetAllAsync();
            //convert domain to DTO 

            var walksDTO = mapper.Map<List<Walk>>(walksdomain);
            //retur n

            return Ok(walksDTO);

        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walkDomain = await walkRepository.GetAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Walk>(walkDomain);
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] AddWalkRequest addWalkRequest)
        {
            // convert DTO to Domain 
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                walkDifficultyId = addWalkRequest.walkDifficultyId
            };
            //pass Domain to repo
            walkDomain = await walkRepository.AddAsync(walkDomain);
            //convert back to DTO
            var walkDTO = mapper.Map<Walk>(walkDomain);
            //send DTO to CLient
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id},walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequest updateWalkRequest)
        {
            //convert DTO to Domain 

            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                walkDifficultyId = updateWalkRequest.walkDifficultyId
            };


            // pass details 

            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            // handle NULL
            if (walkDomain == null)
            {
                return NotFound();
            }

            // convert to DTO 

            var walkDTO = mapper.Map<Walk>(walkDomain);
            //response
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walk = await walkRepository.DeleteAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);


        }
    }
}
