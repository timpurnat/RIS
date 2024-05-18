namespace web.Models
{
    public class Znamka
    {
        public int znamkaID { get; set; }
        public String? ImeFerme { get; set; }
        
        
        public ICollection<Znamka>? Znamke { get; set; }
    }
}