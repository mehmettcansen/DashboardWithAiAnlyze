using System.ComponentModel.DataAnnotations;

namespace AiDashboard.Models
{
    public class Stok
    {
        [Key]
        public string UrunKodu { get; set; }
        public string UrunAdi { get; set; }
        public int MevcutStokMiktari { get; set; }
        public int MinimumStokSeviyesi { get; set; }
        public string MaximumStokSeviyesi { get; set; }
        public string DepoKonumu { get; set; }
        public decimal DepoKiraMaliyeti { get; set; }
        public int StokDevirHizi { get; set; }
        public int StoktaBekleyenGunSayisi { get; set; }
        public int StokMaliyetiBirimBasina { get; set; }
        public decimal ToplamStokMaliyeti { get; set; }
        public int IadeUrunSayisi { get; set; }
        public int DepoPersonelSayisi { get; set; }
    }
}
