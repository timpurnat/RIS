namespace web.Models
{
    public class Kategorija
    {
        public int KategorijaID { get; set; }
        public String? imeKategorije { get; set; }
        public ICollection<Izdelek>? Izdeleki { get; set; }
    }
}