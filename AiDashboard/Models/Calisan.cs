using System.ComponentModel.DataAnnotations;

namespace AiDashboard.Models
{
    public class Calisan
    {
        [Key]
        public string CalisanNo { get; set; }
        public string AdiSoyadi { get; set; }
        public string Pozisyon { get; set; }
        public string Departman { get; set; }
        public DateTime IseGirisTarihi { get; set; }
        public decimal BrutMaas { get; set; }
        public decimal NetMaas { get; set; }
        public decimal PrimIkramiyeler { get; set; }
        public int MesaiSuresi { get; set; }
        public int IzinGunleri { get; set; }
        public int DevamsizlikSayisi { get; set; }
        public decimal PersonelEgitimGiderleri { get; set; }
        public int PersormansDegerlendirmeRekoru { get; set; }
    }
}
