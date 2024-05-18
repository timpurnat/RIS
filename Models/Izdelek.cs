using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public class Izdelek
    {
        public int IzdelekId { get; set; }
        public string? Naslov { get; set; }
        public string ImageLink { get; set; }
        public Znamka? znamka { get; set; }
        public Zvrst? Zvrst { get; set; }
        public Kategorija? Kategorija { get; set; }
        [Range(0, 10)]
        public double? Ocena{ get; set; }
        public int? znamkaID {get; set;}
        public int? ZvrstID { get; set; }
        public int? KategorijaID { get; set; }
        public Rezervacija? Rezervacije { get; set; }
        public string? rezervirano{ get{
            if(Rezervacije != null){
                return "REZERVIRANO";
            }
            return null;
        } }
    
    
    }
}