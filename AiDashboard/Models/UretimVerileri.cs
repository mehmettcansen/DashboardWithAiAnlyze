using System.ComponentModel.DataAnnotations;

namespace AiDashboard.Models
{
    public class UretimVerileri
    {
        [Key]
        public string UretimNo { get; set; }
        public DateTime UretimPlaniTarihi { get; set; }
        public DateTime UretimBaslamaTarihi { get; set; }
        public DateTime UretimTamamlanmaTarih { get; set; }
        public DateTime HedeflenenTamamlanmaTarihi { get; set; }
        public string UrunKodu { get; set; }
        public string UrunAdi { get; set; }
        public int UretimHedefMiktari { get; set; }
        public int CalismaSuresiSaat { get; set; }
        public int GerceklesenUretimMiktari { get; set; }
        public int MakineArizaSuresiDakika { get; set; }
        public decimal AtikMalzemeMiktari { get; set; }
        public int UretimGecikmeSuresiSaat { get; set; }
        public string UretimHattiKodu { get; set; }
        public string MakineKodu { get; set; }
        public int BakimOnarimGiderleri { get; set; }
        public int UretimMaliyetiBirimBasina { get; set; }
        public string KaliteKontrol { get; set; }
        public string KaliteKontrolTestiTuru { get; set; }
    }
}
