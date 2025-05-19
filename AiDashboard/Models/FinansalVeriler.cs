using System.ComponentModel.DataAnnotations;

namespace AiDashboard.Models
{
    public class FinansalVeriler
    {
        [Key]
        public DateTime MuhasebeDonemi { get; set; }
        public decimal ToplamGelir { get; set; }
        public decimal UretimMaliyetleri { get; set; }
        public decimal HammaddeMaliyetleri { get; set; }
        public decimal PersonelGiderleri { get; set; }
        public decimal BakimOnarimGiderleri { get; set; }
        public decimal TedarikMaliyetleri { get; set; }
        public decimal IsletmeGiderleri { get; set; }
        public decimal KiraGiderleri { get; set; }
        public decimal LojistikNakliyeGiderleri { get; set; }
        public decimal ArgeGiderleri { get; set; }
        public decimal PazarlamaReklamGiderleri { get; set; }
        public decimal ToplamGiderler { get; set; }
        public decimal Kar { get; set; }
        public decimal ToplamBorclar { get; set; }
        public decimal ToplamAlacaklar { get; set; }
        public int TahsilatOrani { get; set; }
        public int FaturalandirmaDongusu { get; set; }
        public int BorcOdemeDurumu { get; set; }
    }
}
