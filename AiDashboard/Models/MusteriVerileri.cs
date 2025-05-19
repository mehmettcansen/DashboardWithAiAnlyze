using System.ComponentModel.DataAnnotations;

namespace AiDashboard.Models
{
    public class MusteriVerileri
    {
        [Key]
        public string MusteriId { get; set; }
        public string MusteriAdi { get; set; }
        public string MusteriTuru { get; set; }
        public string FaturaAdresi { get; set; }
        public string TeslimatAdresi { get; set; }
        public string IletisimBilgileri { get; set; }
        public int SiparisGecmisi { get; set; }
        public int ToplamSatinalmaMiktari { get; set; }
        public int IptalEdilenSiparisler { get; set; }
        public int IptalEdilenSiparisMiktari { get; set; }
        public int IadeEdilenSiparisler { get; set; }
        public int IadeEdilenSiparisMiktar { get; set; }
    }
}
