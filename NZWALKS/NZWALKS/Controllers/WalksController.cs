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
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper,IRegionRepository regionRepository,IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
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
            //validation 
            if (await ValidateAddWalkAsync(addWalkRequest) == false)
                return BadRequest(ModelState);
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

            //validation 
            if (await ValidateUpdateWalkAsync(updateWalkRequest) == false)
                return BadRequest(ModelState);
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

        #region Private Methods

        private  async Task<bool> ValidateUpdateWalkAsync(UpdateWalkRequest updateWalk)
        {
            if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), $"{nameof(addWalkRequest)} cannot be null or empty .");
                //return false;
            }
            if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} cannot be white space or empty .");
                //return false;
            }
            if (addWalkRequest.Length < 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} cannot be negative number.");
                //return false;
            }
            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
           
            if (region  == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is invalide.");
                //return false;
            }
            var wlkdifficulty = await walkDifficultyRepository.GetAsync(addWalkRequest.walkDifficultyId);

            if (wlkdifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.walkDifficultyId), $"{nameof(addWalkRequest.walkDifficultyId)} is invalide.");
                //return false;
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateAddWalkAsync(AddWalkRequest updateWalk)
        {
            if (updateWalk == null)
            {
                ModelState.AddModelError(nameof(updateWalk), $"{nameof(updateWalk)} cannot be null or empty .");
                //return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalk.Name))
            {
                ModelState.AddModelError(nameof(updateWalk.Name), $"{nameof(updateWalk.Name)} cannot be white space or empty .");
                //return false;
            }
            if (updateWalk.Length < 0)
            {
                ModelState.AddModelError(nameof(updateWalk.Length), $"{nameof(updateWalk.Length)} cannot be negative number.");
                //return false;
            }
            var region = await regionRepository.GetAsync(updateWalk.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalk.RegionId), $"{nameof(updateWalk.RegionId)} is invalide.");
                //return false;
            }
            var wlkdifficulty = await walkDifficultyRepository.GetAsync(updateWalk.walkDifficultyId);

            if (wlkdifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalk.walkDifficultyId), $"{nameof(updateWalk.walkDifficultyId)} is invalide.");
                //return false;
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
