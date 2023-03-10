namespace NZWALKS.Models.Domain
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

        // Navigation Property 
        public IEnumerable<Walk> walks { get; set; }
    }
}
