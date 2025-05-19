using System.ComponentModel.DataAnnotations;

namespace AiDashboard.Models
{
    public class TedarikVeMalzeme
    {
        [Key]
        public string MalzemeKodu { get; set; }
        public string MalzemeAdi { get; set; }
        public string TedarikciAdi { get; set; }
        public string TedarikciKodu { get; set; }
        public DateTime SiparisTarihi { get; set; }
        public DateTime TeslimTarihi { get; set; }
        public int TeslimSuresiGun { get; set; }
        public int SiparisMiktari { get; set; }
        public int TeslimAlinanMiktar { get; set; }
        public string MalzemeKaliteKontrolDurumu { get; set; }
        public decimal MalzemeFireOrani { get; set; }
        public decimal IadeEdilenMalzemeOrani { get; set; }
        public int StoktaKalanMalzemeMiktari { get; set; }
    }
}
