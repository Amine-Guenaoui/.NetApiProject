using NZWALKS.Models.Domain;

namespace NZWALKS.Repository
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
    }
}
