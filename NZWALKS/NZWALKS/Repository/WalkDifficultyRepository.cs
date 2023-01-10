using Microsoft.EntityFrameworkCore;
using NZWALKS.Data;
using NZWALKS.Models.Domain;

namespace NZWALKS.Repository
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDbContext.WalkDifficulties.ToListAsync();
        }
        public  async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nZWalksDbContext.WalkDifficulties.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDbContext.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
          
            var walk = await nZWalksDbContext.WalkDifficulties.FirstOrDefaultAsync(x => x.Id == id);
            if (walk == null)
            {
                return null;
            }

            nZWalksDbContext.WalkDifficulties.Remove(walk);
            await nZWalksDbContext.SaveChangesAsync();

            return walk;
        }


            public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingwalk = await nZWalksDbContext.WalkDifficulties.FindAsync(id);
            if (existingwalk == null)
            {
                return null;
            }

            existingwalk.Code = walkDifficulty.Code;
            await nZWalksDbContext.SaveChangesAsync();

            return existingwalk;

        }
    }
}
