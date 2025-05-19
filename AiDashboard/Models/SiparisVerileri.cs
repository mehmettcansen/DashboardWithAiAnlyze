using System.ComponentModel.DataAnnotations;

namespace AiDashboard.Models
{
    public class SiparisVerileri
    {
        [Key]
        public string SiparisNo { get; set; }
        public string MusteriAdi { get; set; }
        public DateTime SiparisTarihi { get; set; }
        public DateTime TeslimTarihi { get; set; }
        public string SiparisDurumu { get; set; }
        public string SatinAlinaUrunAdi { get; set; }
        public string SatinAlinanUrunKodu { get; set; }
        public int Adet { get; set; }
        public decimal BirimFiyati { get; set; }
        public decimal ToplamFiyat { get; set; }
        public string SatisKanali { get; set; }
        public string OdemeYontemi { get; set; }
        public string IadeDurumu { get; set; }
        public DateTime? IadeTarihi { get; set; } // NULL olabilir
        public string IadeNedeni { get; set; }
    }
}
