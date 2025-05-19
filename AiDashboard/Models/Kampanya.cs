using System.ComponentModel.DataAnnotations;

namespace AiDashboard.Models
{
    public class Kampanya
    {
        [Key]
        public string KampanyaKodu { get; set; }
        public string KampanyaAdi { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string HedefKitle { get; set; }
        public string ReklamKanali { get; set; }
        public int ReklamButcesi { get; set; }
        public int ErisimSayisi { get; set; }
        public decimal TiklamaOrani { get; set; }
        public decimal SatisaDonusumOrani { get; set; }
        public string Urunler { get; set; }
    }
}
