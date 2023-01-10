namespace NZWALKS.Models.DTO
{
    public class AddRegionRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Area { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public int Population { get; set; }
    }
}
