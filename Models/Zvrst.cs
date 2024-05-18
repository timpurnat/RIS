namespace web.Models
{
    public class Zvrst
    {
        public int ZvrstID { get; set; }
        public String? ImeZvrsti { get; set; }
        public ICollection<Izdelek>? Izdelki { get; set; }
    }
}