namespace NZWALKS.Models.DTO
{
    public class Region
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Area { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public int Population { get; set; }

    }
}
