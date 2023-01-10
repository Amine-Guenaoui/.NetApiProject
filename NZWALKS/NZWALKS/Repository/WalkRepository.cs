using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWALKS.Data;
using NZWALKS.Models.Domain;

namespace NZWALKS.Repository
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;
        private readonly IMapper mapper;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
          
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            // assign new ID 
            walk.Id = Guid.NewGuid();
           await nZWalksDbContext.AddAsync(walk);
           await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

    

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return  await nZWalksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();



        }

        public async Task<Walk> GetAsync(Guid id) => await nZWalksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingwalk = await nZWalksDbContext.Walks.FindAsync(id);
            if (existingwalk != null)
            {
                existingwalk.Length = walk.Length;
                existingwalk.Name= walk.Name;
                existingwalk.RegionId= walk.RegionId;
                existingwalk.walkDifficultyId= walk.walkDifficultyId;
                await nZWalksDbContext.SaveChangesAsync();
                return existingwalk;
            }


            return null;

        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var walk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walk == null)
            {
                return null;
            }

            nZWalksDbContext.Walks.Remove(walk);
            await nZWalksDbContext.SaveChangesAsync();

            return walk;
        }
    }
}
