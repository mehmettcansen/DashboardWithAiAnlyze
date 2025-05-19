using Microsoft.AspNetCore.Mvc;
using AiDashboard.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace AiDashboard.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(AppDbContext context, ILogger<DashboardController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FinansalVeriler()
        {
            return View();
        }

        public IActionResult Calisanlar()
        {
            return View();
        }

        public IActionResult StokDurumu()
        {
            return View();
        }

        public IActionResult TestConnection()
        {
            try
            {
                var count = _context.FinansalVeriler.Count();
                return Content($"FinansalVeriler tablosunda {count} kayıt var.");
            }
            catch (Exception ex)
            {
                return Content($"Hata: {ex.Message}");
            }
        }

        public IActionResult GetGelirGiderData()
        {
            try
            {
                // Veritabanındaki tarih aralığına göre startDate'i ayarlıyoruz (örneğin, 2019 ve sonrası)
                var startDate = new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var rawData = _context.FinansalVeriler
                    .Where(f => f.MuhasebeDonemi >= startDate) // Veri miktarını sınırlandırıyoruz
                    .GroupBy(f => f.MuhasebeDonemi)
                    .Select(g => new
                    {
                        Tarih = g.Key, // DateTime olarak çekiyoruz
                        Gelir = g.Sum(x => x.ToplamGelir),
                        Gider = g.Sum(x => x.PersonelGiderleri + x.UretimMaliyetleri + x.HammaddeMaliyetleri + x.BakimOnarimGiderleri + x.TedarikMaliyetleri + x.IsletmeGiderleri + x.KiraGiderleri + x.LojistikNakliyeGiderleri + x.ArgeGiderleri + x.PazarlamaReklamGiderleri)
                    })
                    .OrderBy(x => x.Tarih)
                    .AsEnumerable() // Veriyi istemci tarafına çekiyoruz
                    .Select(x => new
                    {
                        Tarih = x.Tarih.ToString("yyyy-MM"), // İstemci tarafında formatlıyoruz
                        x.Gelir,
                        x.Gider
                    })
                    .ToList();

                // Veri boşsa boş bir chartData döndür
                if (!rawData.Any())
                {
                    _logger.LogWarning("FinansalVeriler tablosunda veri bulunamadı.");
                    return Json(new { labels = new string[0], datasets = new object[0] });
                }

                var chartData = new
                {
                    labels = rawData.Select(d => d.Tarih).ToArray(),
                    datasets = new[]
                    {
                        new { label = "Gelir", data = rawData.Select(d => d.Gelir).ToArray(), borderColor = "#34d399", fill = false },
                        new { label = "Gider", data = rawData.Select(d => d.Gider).ToArray(), borderColor = "#f87171", fill = false }
                    }
                };

                return Json(chartData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Veri çekme sırasında bir hata oluştu.");
                return StatusCode(500, new { error = $"Veri çekme hatası: {ex.Message}" });
            }
        }

        public IActionResult GetGiderDagilimData()
        {
            try
            {
                // En son dönemi al
                var sonDonem = _context.FinansalVeriler
                    .OrderByDescending(f => f.MuhasebeDonemi)
                    .FirstOrDefault();

                if (sonDonem == null)
                {
                    _logger.LogWarning("FinansalVeriler tablosunda veri bulunamadı.");
                    return Json(new { labels = new string[0], datasets = new object[0] });
                }

                // Gider kategorilerini ve değerlerini hazırla
                var giderler = new List<(string Kategori, decimal Deger)>
                {
                    ("Personel Giderleri", sonDonem.PersonelGiderleri),
                    ("Üretim Maliyetleri", sonDonem.UretimMaliyetleri),
                    ("Hammadde Maliyetleri", sonDonem.HammaddeMaliyetleri),
                    ("Bakım Onarım Giderleri", sonDonem.BakimOnarimGiderleri),
                    ("Tedarik Maliyetleri", sonDonem.TedarikMaliyetleri),
                    ("İşletme Giderleri", sonDonem.IsletmeGiderleri),
                    ("Kira Giderleri", sonDonem.KiraGiderleri),
                    ("Lojistik/Nakliye Giderleri", sonDonem.LojistikNakliyeGiderleri),
                    ("AR-GE Giderleri", sonDonem.ArgeGiderleri),
                    ("Pazarlama/Reklam Giderleri", sonDonem.PazarlamaReklamGiderleri)
                };

                // Boş olanları filtrele
                giderler = giderler
                    .Where(g => g.Deger > 0)
                    .OrderByDescending(g => g.Deger)
                    .ToList();

                var chartData = new
                {
                    labels = giderler.Select(g => g.Kategori).ToArray(),
                    datasets = new[]
                    {
                        new {
                            data = giderler.Select(g => g.Deger).ToArray(),
                            backgroundColor = new[] {
                                "#60a5fa", // Mavi
                                "#f87171", // Kırmızı
                                "#34d399", // Yeşil
                                "#fbbf24", // Sarı
                                "#a78bfa", // Mor
                                "#f472b6", // Pembe
                                "#3b82f6", // Koyu Mavi
                                "#10b981", // Koyu Yeşil
                                "#ef4444", // Koyu Kırmızı
                                "#8b5cf6"  // Koyu Mor
                            }
                        }
                    }
                };

                return Json(chartData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gider dağılım verisi çekme sırasında bir hata oluştu.");
                return StatusCode(500, new { error = $"Veri çekme hatası: {ex.Message}" });
            }
        }

        public IActionResult GetKarZararData()
        {
            try
            {
                // Veritabanındaki tarih aralığına göre startDate'i ayarlıyoruz (örneğin, 2019 ve sonrası)
                var startDate = new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var rawData = _context.FinansalVeriler
                    .Where(f => f.MuhasebeDonemi >= startDate) // Veri miktarını sınırlandırıyoruz
                    .OrderBy(f => f.MuhasebeDonemi)
                    .Select(f => new
                    {
                        Tarih = f.MuhasebeDonemi,
                        Kar = f.Kar
                    })
                    .AsEnumerable() // Veriyi istemci tarafına çekiyoruz
                    .Select(x => new
                    {
                        Tarih = x.Tarih.ToString("yyyy-MM"), // İstemci tarafında formatlıyoruz
                        x.Kar
                    })
                    .ToList();

                // Veri boşsa boş bir chartData döndür
                if (!rawData.Any())
                {
                    _logger.LogWarning("FinansalVeriler tablosunda kâr/zarar verisi bulunamadı.");
                    return Json(new { labels = new string[0], datasets = new object[0] });
                }

                var chartData = new
                {
                    labels = rawData.Select(d => d.Tarih).ToArray(),
                    datasets = new[]
                    {
                        new { 
                            label = "Kâr/Zarar", 
                            data = rawData.Select(d => d.Kar).ToArray(), 
                            borderColor = "#60a5fa", 
                            fill = false 
                        }
                    }
                };

                return Json(chartData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kâr/Zarar verisi çekme sırasında bir hata oluştu.");
                return StatusCode(500, new { error = $"Veri çekme hatası: {ex.Message}" });
            }
        }

        public IActionResult GetBorcAlacakData()
        {
            try
            {
                // Veritabanındaki tarih aralığına göre startDate'i ayarlıyoruz (örneğin, 2019 ve sonrası)
                var startDate = new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var rawData = _context.FinansalVeriler
                    .Where(f => f.MuhasebeDonemi >= startDate) // Veri miktarını sınırlandırıyoruz
                    .OrderBy(f => f.MuhasebeDonemi)
                    .Select(f => new
                    {
                        Tarih = f.MuhasebeDonemi,
                        ToplamBorclar = f.ToplamBorclar,
                        ToplamAlacaklar = f.ToplamAlacaklar
                    })
                    .AsEnumerable() // Veriyi istemci tarafına çekiyoruz
                    .Select(x => new
                    {
                        Tarih = x.Tarih.ToString("yyyy-MM"), // İstemci tarafında formatlıyoruz
                        x.ToplamBorclar,
                        x.ToplamAlacaklar
                    })
                    .ToList();

                // Veri boşsa boş bir chartData döndür
                if (!rawData.Any())
                {
                    _logger.LogWarning("FinansalVeriler tablosunda borç/alacak verisi bulunamadı.");
                    return Json(new { labels = new string[0], datasets = new object[0] });
                }

                var chartData = new
                {
                    labels = rawData.Select(d => d.Tarih).ToArray(),
                    datasets = new[]
                    {
                        new { 
                            label = "Toplam Borçlar", 
                            data = rawData.Select(d => d.ToplamBorclar).ToArray(), 
                            borderColor = "#f87171", // kırmızı
                            fill = false 
                        },
                        new { 
                            label = "Toplam Alacaklar", 
                            data = rawData.Select(d => d.ToplamAlacaklar).ToArray(), 
                            borderColor = "#34d399", // yeşil
                            fill = false 
                        }
                    }
                };

                return Json(chartData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Borç/Alacak verisi çekme sırasında bir hata oluştu.");
                return StatusCode(500, new { error = $"Veri çekme hatası: {ex.Message}" });
            }
        }

        public IActionResult GetStokMiktarlariData()
        {
            try
            {
                var stokVerileri = _context.Stok
                    .OrderByDescending(s => s.MevcutStokMiktari)
                    .Take(15) // En yüksek stok miktarına sahip 15 ürün
                    .Select(s => new
                    {
                        UrunKodu = s.UrunKodu,
                        UrunAdi = s.UrunAdi,
                        StokMiktari = s.MevcutStokMiktari
                    })
                    .ToList();

                // Veri boşsa boş bir chartData döndür
                if (!stokVerileri.Any())
                {
                    _logger.LogWarning("Stok tablosunda veri bulunamadı.");
                    return Json(new { labels = new string[0], datasets = new object[0] });
                }

                var chartData = new
                {
                    labels = stokVerileri.Select(d => d.UrunAdi).ToArray(),
                    datasets = new[]
                    {
                        new { 
                            label = "Stok Miktarı", 
                            data = stokVerileri.Select(d => d.StokMiktari).ToArray(),
                            backgroundColor = new[] {
                                "#60a5fa", // Mavi
                                "#f87171", // Kırmızı
                                "#34d399", // Yeşil
                                "#fbbf24", // Sarı
                                "#a78bfa", // Mor
                                "#f472b6", // Pembe
                                "#3b82f6", // Koyu Mavi
                                "#10b981", // Koyu Yeşil
                                "#ef4444", // Koyu Kırmızı
                                "#8b5cf6", // Koyu Mor
                                "#fb923c", // Turuncu
                                "#4ade80", // Açık Yeşil
                                "#22d3ee", // Turkuaz
                                "#e879f9", // Açık Mor
                                "#fb7185"  // Açık Kırmızı
                            },
                            borderColor = new[] {
                                "#3b82f6", // Mavi
                                "#ef4444", // Kırmızı
                                "#10b981", // Yeşil  
                                "#f59e0b", // Sarı
                                "#8b5cf6", // Mor
                                "#ec4899", // Pembe
                                "#1d4ed8", // Koyu Mavi
                                "#047857", // Koyu Yeşil
                                "#b91c1c", // Koyu Kırmızı
                                "#7c3aed", // Koyu Mor
                                "#ea580c", // Turuncu
                                "#16a34a", // Açık Yeşil
                                "#0891b2", // Turkuaz
                                "#d946ef", // Açık Mor
                                "#e11d48"  // Açık Kırmızı
                            },
                            borderWidth = 1
                        }
                    }
                };

                return Json(chartData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stok miktarları verisi çekme sırasında bir hata oluştu.");
                return StatusCode(500, new { error = $"Veri çekme hatası: {ex.Message}" });
            }
        }

        public IActionResult GetStokDevirHiziData()
        {
            try
            {
                var stokDevirHiziVerileri = _context.Stok
                    .OrderByDescending(s => s.StokDevirHizi)
                    .Take(20) // En yüksek stok devir hızına sahip 20 ürün
                    .Select(s => new
                    {
                        UrunKodu = s.UrunKodu,
                        UrunAdi = s.UrunAdi,
                        DevirHizi = s.StokDevirHizi
                    })
                    .ToList();

                // Veri boşsa boş bir chartData döndür
                if (!stokDevirHiziVerileri.Any())
                {
                    _logger.LogWarning("Stok tablosunda devir hızı verisi bulunamadı.");
                    return Json(new { labels = new string[0], datasets = new object[0] });
                }

                var chartData = new
                {
                    labels = stokDevirHiziVerileri.Select(d => d.UrunAdi.Length > 25 ? d.UrunAdi.Substring(0, 22) + "..." : d.UrunAdi).ToArray(),
                    datasets = new[]
                    {
                        new { 
                            label = "Stok Devir Hızı", 
                            data = stokDevirHiziVerileri.Select(d => d.DevirHizi).ToArray(),
                            backgroundColor = new[] {
                                "#60a5fa", // Mavi
                                "#34d399", // Yeşil
                                "#f87171", // Kırmızı
                                "#fbbf24", // Sarı
                                "#a78bfa", // Mor
                                "#4ade80", // Açık Yeşil
                                "#f472b6", // Pembe
                                "#fb923c", // Turuncu
                                "#22d3ee", // Turkuaz
                                "#3b82f6", // Koyu Mavi
                                "#10b981", // Koyu Yeşil
                                "#ef4444", // Koyu Kırmızı
                                "#8b5cf6", // Koyu Mor
                                "#e879f9", // Açık Mor
                                "#fb7185", // Açık Kırmızı
                                "#14b8a6", // Teal
                                "#facc15", // Altın
                                "#06b6d4", // Cyan
                                "#d946ef", // Fuşya
                                "#6366f1"  // Indigo
                            }, 
                            borderColor = "#3b82f6",
                            borderWidth = 1
                        }
                    }
                };

                return Json(chartData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stok devir hızı verisi çekme sırasında bir hata oluştu.");
                return StatusCode(500, new { error = $"Veri çekme hatası: {ex.Message}" });
            }
        }

        public IActionResult GetDepartmanCalisanSayisiData()
        {
            try
            {
                var departmanVerisi = _context.Calisan
                    .GroupBy(c => c.Departman)
                    .Select(g => new
                    {
                        Departman = g.Key,
                        CalisanSayisi = g.Count()
                    })
                    .OrderByDescending(x => x.CalisanSayisi)
                    .ToList();

                // Veri boşsa boş bir chartData döndür
                if (!departmanVerisi.Any())
                {
                    _logger.LogWarning("Calisan tablosunda veri bulunamadı.");
                    return Json(new { labels = new string[0], datasets = new object[0] });
                }

                var chartData = new
                {
                    labels = departmanVerisi.Select(d => d.Departman).ToArray(),
                    datasets = new[]
                    {
                        new {
                            label = "Çalışan Sayısı",
                            data = departmanVerisi.Select(d => d.CalisanSayisi).ToArray(),
                            backgroundColor = new[] {
                                "#60a5fa", // Mavi
                                "#f87171", // Kırmızı
                                "#34d399", // Yeşil
                                "#fbbf24", // Sarı
                                "#a78bfa", // Mor
                                "#f472b6", // Pembe
                                "#3b82f6", // Koyu Mavi
                                "#10b981", // Koyu Yeşil
                                "#ef4444", // Koyu Kırmızı
                                "#8b5cf6"  // Koyu Mor
                            }
                        }
                    }
                };

                return Json(chartData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Departman çalışan sayısı verisi çekme sırasında bir hata oluştu.");
                return StatusCode(500, new { error = $"Veri çekme hatası: {ex.Message}" });
            }
        }

        public IActionResult GetCalisanPerformansData()
        {
            try
            {
                var performansVerisi = _context.Calisan
                    .GroupBy(c => c.Departman)
                    .Select(g => new
                    {
                        Departman = g.Key,
                        OrtalamaPerfomans = g.Average(c => c.PersormansDegerlendirmeRekoru)
                    })
                    .OrderByDescending(x => x.OrtalamaPerfomans)
                    .ToList();

                // Veri boşsa boş bir chartData döndür
                if (!performansVerisi.Any())
                {
                    _logger.LogWarning("Calisan tablosunda performans verisi bulunamadı.");
                    return Json(new { labels = new string[0], datasets = new object[0] });
                }

                var chartData = new
                {
                    labels = performansVerisi.Select(d => d.Departman).ToArray(),
                    datasets = new[]
                    {
                        new {
                            label = "Ortalama Performans",
                            data = performansVerisi.Select(d => d.OrtalamaPerfomans).ToArray(),
                            backgroundColor = new[] {
                                "#34d399", // Yeşil
                                "#60a5fa", // Mavi
                                "#fbbf24", // Sarı
                                "#f87171", // Kırmızı
                                "#a78bfa", // Mor
                                "#f472b6", // Pembe
                                "#3b82f6", // Koyu Mavi
                                "#10b981", // Koyu Yeşil
                                "#ef4444", // Koyu Kırmızı
                                "#8b5cf6"  // Koyu Mor
                            }
                        }
                    }
                };

                return Json(chartData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Çalışan performans ortalamaları verisi çekme sırasında bir hata oluştu.");
                return StatusCode(500, new { error = $"Veri çekme hatası: {ex.Message}" });
            }
        }

        public IActionResult GetCalisanMaasDagilimData()
        {
            try
            {
                var maasDagilimVerisi = _context.Calisan
                    .GroupBy(c => c.Departman)
                    .Select(g => new
                    {
                        Departman = g.Key,
                        OrtalamaMaas = g.Average(c => c.NetMaas)
                    })
                    .OrderByDescending(x => x.OrtalamaMaas)
                    .ToList();

                // Veri boşsa boş bir chartData döndür
                if (!maasDagilimVerisi.Any())
                {
                    _logger.LogWarning("Calisan tablosunda maaş verisi bulunamadı.");
                    return Json(new { labels = new string[0], datasets = new object[0] });
                }

                var chartData = new
                {
                    labels = maasDagilimVerisi.Select(d => d.Departman).ToArray(),
                    datasets = new[]
                    {
                        new {
                            label = "Ortalama Maaş",
                            data = maasDagilimVerisi.Select(d => d.OrtalamaMaas).ToArray(),
                            backgroundColor = new[] {
                                "#fbbf24", // Sarı
                                "#60a5fa", // Mavi
                                "#34d399", // Yeşil
                                "#f87171", // Kırmızı
                                "#a78bfa", // Mor
                                "#f472b6", // Pembe
                                "#3b82f6", // Koyu Mavi
                                "#10b981", // Koyu Yeşil
                                "#ef4444", // Koyu Kırmızı
                                "#8b5cf6"  // Koyu Mor
                            }
                        }
                    }
                };

                return Json(chartData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Çalışan maaş dağılımı verisi çekme sırasında bir hata oluştu.");
                return StatusCode(500, new { error = $"Veri çekme hatası: {ex.Message}" });
            }
        }
    }
}