using Microsoft.EntityFrameworkCore;
using AiDashboard.Models;

namespace AiDashboard.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet'ler: Her model için bir DbSet tanımlıyoruz
        public DbSet<Calisan> Calisan { get; set; }
        public DbSet<FinansalVeriler> FinansalVeriler { get; set; }
        public DbSet<Kampanya> Kampanya { get; set; }
        public DbSet<MusteriVerileri> MusteriVerileri { get; set; }
        public DbSet<SiparisVerileri> SiparisVerileri { get; set; }
        public DbSet<Stok> Stok { get; set; }
        public DbSet<TedarikVeMalzeme> TedarikVeMalzeme { get; set; }
        public DbSet<UretimVerileri> UretimVerileri { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tablo adlarını veritabanındaki isimlerle eşleştirme
            modelBuilder.Entity<Calisan>().ToTable("calisanlar");
            modelBuilder.Entity<FinansalVeriler>().ToTable("finansal_veriler");
            modelBuilder.Entity<Kampanya>().ToTable("kampanya");
            modelBuilder.Entity<MusteriVerileri>().ToTable("musteri_verileri");
            modelBuilder.Entity<SiparisVerileri>().ToTable("siparis_verileri");
            modelBuilder.Entity<Stok>().ToTable("stok_ve_envanter_verileri");
            modelBuilder.Entity<TedarikVeMalzeme>().ToTable("tedarik_ve_malzeme");
            modelBuilder.Entity<UretimVerileri>().ToTable("uretim_verileri");

            // Calisan sütun eşleştirmeleri
            modelBuilder.Entity<Calisan>(entity =>
            {
                entity.Property(e => e.CalisanNo).HasColumnName("calisan_no");
                entity.Property(e => e.AdiSoyadi).HasColumnName("adi_soyadi");
                entity.Property(e => e.Pozisyon).HasColumnName("pozisyon");
                entity.Property(e => e.Departman).HasColumnName("departman");
                entity.Property(e => e.IseGirisTarihi).HasColumnName("ise_giris_tarihi");
                entity.Property(e => e.BrutMaas).HasColumnName("brut_maas");
                entity.Property(e => e.NetMaas).HasColumnName("net_maas");
                entity.Property(e => e.PrimIkramiyeler).HasColumnName("prim_ikramiyeler");
                entity.Property(e => e.MesaiSuresi).HasColumnName("mesai_suresi");
                entity.Property(e => e.IzinGunleri).HasColumnName("izin_gunleri");
                entity.Property(e => e.DevamsizlikSayisi).HasColumnName("devamsizlik_sayisi");
                entity.Property(e => e.PersonelEgitimGiderleri).HasColumnName("personel_egitim_giderleri");
                entity.Property(e => e.PersormansDegerlendirmeRekoru).HasColumnName("performans_degerlendirme_rekoru");
            });

            // FinansalVeriler sütun eşleştirmeleri
            modelBuilder.Entity<FinansalVeriler>(entity =>
            {
                entity.Property(e => e.MuhasebeDonemi).HasColumnName("muhasebe_donemi");
                entity.Property(e => e.ToplamGelir).HasColumnName("toplam_gelir");
                entity.Property(e => e.UretimMaliyetleri).HasColumnName("uretim_maliyetleri");
                entity.Property(e => e.HammaddeMaliyetleri).HasColumnName("hammadde_maliyetleri");
                entity.Property(e => e.PersonelGiderleri).HasColumnName("personel_giderleri");
                entity.Property(e => e.BakimOnarimGiderleri).HasColumnName("bakim_onarim_giderleri");
                entity.Property(e => e.TedarikMaliyetleri).HasColumnName("tedarik_maliyetleri");
                entity.Property(e => e.IsletmeGiderleri).HasColumnName("isletme_giderleri");
                entity.Property(e => e.KiraGiderleri).HasColumnName("kira_giderleri");
                entity.Property(e => e.LojistikNakliyeGiderleri).HasColumnName("lojistik_nakliye_giderleri");
                entity.Property(e => e.ArgeGiderleri).HasColumnName("arge_giderleri");
                entity.Property(e => e.PazarlamaReklamGiderleri).HasColumnName("pazarlama_reklam_giderleri");
                entity.Property(e => e.ToplamGiderler).HasColumnName("toplam_giderler");
                entity.Property(e => e.Kar).HasColumnName("kar");
                entity.Property(e => e.ToplamBorclar).HasColumnName("toplam_borclar");
                entity.Property(e => e.ToplamAlacaklar).HasColumnName("toplam_alacaklar");
                entity.Property(e => e.TahsilatOrani).HasColumnName("tahsilat_orani");
                entity.Property(e => e.FaturalandirmaDongusu).HasColumnName("faturalandirma_dongusu");
                entity.Property(e => e.BorcOdemeDurumu).HasColumnName("borc_odeme_durumu");
            });

            // Kampanya sütun eşleştirmeleri
            modelBuilder.Entity<Kampanya>(entity =>
            {
                entity.Property(e => e.KampanyaKodu).HasColumnName("kampanya_kodu");
                entity.Property(e => e.KampanyaAdi).HasColumnName("kampanya_adi");
                entity.Property(e => e.BaslangicTarihi).HasColumnName("baslangic_tarihi");
                entity.Property(e => e.BitisTarihi).HasColumnName("bitis_tarihi");
                entity.Property(e => e.HedefKitle).HasColumnName("hedef_kitle");
                entity.Property(e => e.ReklamKanali).HasColumnName("reklam_kanali");
                entity.Property(e => e.ReklamButcesi).HasColumnName("reklam_butcesi");
                entity.Property(e => e.ErisimSayisi).HasColumnName("erisim_sayisi");
                entity.Property(e => e.TiklamaOrani).HasColumnName("tiklama_orani");
                entity.Property(e => e.SatisaDonusumOrani).HasColumnName("satisa_donusum_orani");
                entity.Property(e => e.Urunler).HasColumnName("urunler");
            });

            // MusteriVerileri sütun eşleştirmeleri
            modelBuilder.Entity<MusteriVerileri>(entity =>
            {
                entity.Property(e => e.MusteriId).HasColumnName("musteri_id");
                entity.Property(e => e.MusteriAdi).HasColumnName("musteri_adi");
                entity.Property(e => e.MusteriTuru).HasColumnName("musteri_turu");
                entity.Property(e => e.FaturaAdresi).HasColumnName("fatura_adresi");
                entity.Property(e => e.TeslimatAdresi).HasColumnName("teslimat_adresi");
                entity.Property(e => e.IletisimBilgileri).HasColumnName("iletisim_bilgileri");
                entity.Property(e => e.SiparisGecmisi).HasColumnName("siparis_gecmisi");
                entity.Property(e => e.ToplamSatinalmaMiktari).HasColumnName("toplam_satinalma_miktari");
                entity.Property(e => e.IptalEdilenSiparisler).HasColumnName("iptal_edilen_siparisler");
                entity.Property(e => e.IptalEdilenSiparisMiktari).HasColumnName("iptal_edilen_siparis_miktari");
                entity.Property(e => e.IadeEdilenSiparisler).HasColumnName("iade_edilen_siparisler");
                entity.Property(e => e.IadeEdilenSiparisMiktar).HasColumnName("iade_edilen_siparis_miktar");
            });

            // SiparisVerileri sütun eşleştirmeleri
            modelBuilder.Entity<SiparisVerileri>(entity =>
            {
                entity.Property(e => e.SiparisNo).HasColumnName("siparis_no");
                entity.Property(e => e.MusteriAdi).HasColumnName("musteri_adi");
                entity.Property(e => e.SiparisTarihi).HasColumnName("siparis_tarihi");
                entity.Property(e => e.TeslimTarihi).HasColumnName("teslim_tarihi");
                entity.Property(e => e.SiparisDurumu).HasColumnName("siparis_durumu");
                entity.Property(e => e.SatinAlinaUrunAdi).HasColumnName("satin_alina_urun_adi");
                entity.Property(e => e.SatinAlinanUrunKodu).HasColumnName("satin_alinan_urun_kodu");
                entity.Property(e => e.Adet).HasColumnName("adet");
                entity.Property(e => e.BirimFiyati).HasColumnName("birim_fiyati");
                entity.Property(e => e.ToplamFiyat).HasColumnName("toplam_fiyat");
                entity.Property(e => e.SatisKanali).HasColumnName("satis_kanali");
                entity.Property(e => e.OdemeYontemi).HasColumnName("odeme_yontemi");
                entity.Property(e => e.IadeDurumu).HasColumnName("iade_durumu");
                entity.Property(e => e.IadeTarihi).HasColumnName("iade_tarihi");
                entity.Property(e => e.IadeNedeni).HasColumnName("iade_nedeni");
            });

            // Stok sütun eşleştirmeleri
            modelBuilder.Entity<Stok>(entity =>
            {
                entity.Property(e => e.UrunKodu).HasColumnName("urun_kodu");
                entity.Property(e => e.UrunAdi).HasColumnName("urun_adi");
                entity.Property(e => e.MevcutStokMiktari).HasColumnName("mevcut_stok_miktari");
                entity.Property(e => e.MinimumStokSeviyesi).HasColumnName("minimum_stok_seviyesi");
                entity.Property(e => e.MaximumStokSeviyesi).HasColumnName("maximum_stok_seviyesi");
                entity.Property(e => e.DepoKonumu).HasColumnName("depo_konumu");
                entity.Property(e => e.DepoKiraMaliyeti).HasColumnName("depo_kira_maliyeti");
                entity.Property(e => e.StokDevirHizi).HasColumnName("stok_devir_hizi");
                entity.Property(e => e.StoktaBekleyenGunSayisi).HasColumnName("stokta_bekleyen_gun_sayisi");
                entity.Property(e => e.StokMaliyetiBirimBasina).HasColumnName("stok_maliyeti_birim_basina");
                entity.Property(e => e.ToplamStokMaliyeti).HasColumnName("toplam_stok_maliyeti");
                entity.Property(e => e.IadeUrunSayisi).HasColumnName("iade_urun_sayisi");
                entity.Property(e => e.DepoPersonelSayisi).HasColumnName("depo_personel_sayisi");
            });

            // TedarikVeMalzeme sütun eşleştirmeleri
            modelBuilder.Entity<TedarikVeMalzeme>(entity =>
            {
                entity.Property(e => e.MalzemeKodu).HasColumnName("malzeme_kodu");
                entity.Property(e => e.MalzemeAdi).HasColumnName("malzeme_adi");
                entity.Property(e => e.TedarikciAdi).HasColumnName("tedarikci_adi");
                entity.Property(e => e.TedarikciKodu).HasColumnName("tedarikci_kodu");
                entity.Property(e => e.SiparisTarihi).HasColumnName("siparis_tarihi");
                entity.Property(e => e.TeslimTarihi).HasColumnName("teslim_tarihi");
                entity.Property(e => e.TeslimSuresiGun).HasColumnName("teslim_suresi_gun");
                entity.Property(e => e.SiparisMiktari).HasColumnName("siparis_miktari");
                entity.Property(e => e.TeslimAlinanMiktar).HasColumnName("teslim_alinan_miktar");
                entity.Property(e => e.MalzemeKaliteKontrolDurumu).HasColumnName("malzeme_kalite_kontrol_durumu");
                entity.Property(e => e.MalzemeFireOrani).HasColumnName("malzeme_fire_orani");
                entity.Property(e => e.IadeEdilenMalzemeOrani).HasColumnName("iade_edilen_malzeme_orani");
                entity.Property(e => e.StoktaKalanMalzemeMiktari).HasColumnName("stokta_kalan_malzeme_miktari");
            });

            // UretimVerileri sütun eşleştirmeleri
            modelBuilder.Entity<UretimVerileri>(entity =>
            {
                entity.Property(e => e.UretimNo).HasColumnName("uretim_no");
                entity.Property(e => e.UretimPlaniTarihi).HasColumnName("uretim_plani_tarihi");
                entity.Property(e => e.UretimBaslamaTarihi).HasColumnName("uretim_baslama_tarihi");
                entity.Property(e => e.UretimTamamlanmaTarih).HasColumnName("uretim_tamamlanma_tarih");
                entity.Property(e => e.HedeflenenTamamlanmaTarihi).HasColumnName("hedeflenen_tamamlanma_tarihi");
                entity.Property(e => e.UrunKodu).HasColumnName("urun_kodu");
                entity.Property(e => e.UrunAdi).HasColumnName("urun_adi");
                entity.Property(e => e.UretimHedefMiktari).HasColumnName("uretim_hedef_miktari");
                entity.Property(e => e.CalismaSuresiSaat).HasColumnName("calisma_suresi_saat");
                entity.Property(e => e.GerceklesenUretimMiktari).HasColumnName("gerceklesen_uretim_miktari");
                entity.Property(e => e.MakineArizaSuresiDakika).HasColumnName("makine_ariza_suresi_dakika");
                entity.Property(e => e.AtikMalzemeMiktari).HasColumnName("atik_malzeme_miktari");
                entity.Property(e => e.UretimGecikmeSuresiSaat).HasColumnName("uretim_gecikme_suresi_saat");
                entity.Property(e => e.UretimHattiKodu).HasColumnName("uretim_hatti_kodu");
                entity.Property(e => e.MakineKodu).HasColumnName("makine_kodu");
                entity.Property(e => e.BakimOnarimGiderleri).HasColumnName("bakim_onarim_giderleri");
                entity.Property(e => e.UretimMaliyetiBirimBasina).HasColumnName("uretim_maliyeti_birim_basina");
                entity.Property(e => e.KaliteKontrol).HasColumnName("kalite_kontrol");
                entity.Property(e => e.KaliteKontrolTestiTuru).HasColumnName("kalite_kontrol_testi_turu");
            });
        }
    }
}