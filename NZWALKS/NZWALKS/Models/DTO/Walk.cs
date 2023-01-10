namespace NZWALKS.Models.DTO
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public double Length { get; set; }
        public Guid RegionId { get; set; }
        public Guid walkDifficultyId { get; set; }

        //Navigation Properties 
        public Region Region { get; set; }
        public WalkDifficulty WalkDifficulty { get; set; }
    }
}