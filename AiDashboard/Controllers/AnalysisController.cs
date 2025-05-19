using AiDashboard.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AiDashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly OllamaService _ollamaService;

        public AnalysisController(OllamaService ollamaService)
        {
            _ollamaService = ollamaService;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeData([FromBody] AnalysisRequest request)
        {
            var prompt = CreatePrompt(request.ChartType, request.Data);
            var analysis = await _ollamaService.GenerateAnalysisAsync(prompt);
            return Ok(new { analysis });
        }

        private string CreatePrompt(string chartType, object data)
        {
            return chartType switch
            {
                "gelir_gider" => $"""
                Aşağıdaki gelir-gider verilerini analiz edip sadece ama sadece Türkçe yorum yap:
                {JsonSerializer.Serialize(data)}
                Önemli trendleri ve anormallikleri vurgula.
                Kısa ve öz ol, maksimum 3 paragraf.
                """,
                "gider_dagitim" => $"""
                Aşağıdaki gider dağılımı verilerini analiz edip sadece ama sadece Türkçe yorum yap:
                {JsonSerializer.Serialize(data)}
                Hangi gider kategorilerinin en yüksek olduğunu ve bunların toplam giderler içindeki oranını belirt.
                Maliyetleri azaltmak için potansiyel alanları öner.
                Kısa ve öz ol, maksimum 3 paragraf.
                """,
                "kar_zarar" => $"""
                Aşağıdaki kâr/zarar verilerini analiz edip sadece ama sadece Türkçe yorum yap:
                {JsonSerializer.Serialize(data)}
                Kâr/zarar trendini değerlendir, önemli artış veya düşüşleri ve olası nedenleri belirt.
                Şirketin finansal performansı hakkında genel bir yorum yap.
                Kısa ve öz ol, maksimum 3 paragraf.
                """,
                "borc_alacak" => $"""
                Aşağıdaki borç ve alacak trendleri verilerini analiz edip sadece ama sadece Türkçe yorum yap:
                {JsonSerializer.Serialize(data)}
                Borç-alacak dengesini değerlendir ve nakit akışına etkilerini belirt.
                Risk oluşturabilecek durumları ve iyileştirme önerilerini açıkla.
                Kısa ve öz ol, maksimum 3 paragraf.
                """,
                "departman_calisan_sayisi" => $"""
                Aşağıdaki departman bazında çalışan sayısı verilerini analiz edip sadece ama sadece Türkçe yorum yap:
                {JsonSerializer.Serialize(data)}
                Departmanlar arasındaki çalışan dağılımını değerlendir.
                İnsan kaynakları planlaması ve organizasyon yapısı hakkında öneriler sun.
                Kısa ve öz ol, maksimum 3 paragraf.
                """,
                "calisan_performans" => $"""
                Aşağıdaki departman bazında çalışan performans ortalamaları verilerini analiz edip sadece ama sadece Türkçe yorum yap:
                {JsonSerializer.Serialize(data)}
                Departmanlar arası performans farklılıklarını değerlendir.
                Düşük performanslı alanlar için iyileştirme önerileri ve yüksek performanslı alanların başarı faktörlerini belirt.
                Kısa ve öz ol, maksimum 3 paragraf.
                """,
                "calisan_maas" => $"""
                Aşağıdaki departman bazında ortalama maaş verilerini analiz edip sadece ama sadece Türkçe yorum yap:
                {JsonSerializer.Serialize(data)}
                Departmanlar arası ücret farklılıklarını değerlendir.
                Şirketin maaş politikası, eşitlik, adalet ve performans ile ilişkisi açısından değerlendirme yap.
                Kısa ve öz ol, maksimum 3 paragraf.
                """,
                "stok_miktarlari" => $"""
                Aşağıdaki ürün bazında stok miktarları verilerini analiz edip sadece ama sadece Türkçe yorum yap:
                {JsonSerializer.Serialize(data)}
                Stok dağılımını değerlendir, hangi ürünlerin stokta fazla veya az olduğunu belirt.
                Stok yönetimi optimizasyonu için öneriler sun ve potansiyel risk veya fırsatları işaret et.
                Kısa, öz ve profesyonel bir şekilde analiz yap, maksimum 3 paragraf.
                """,
                "stok_devir_hizi" => $"""
                Aşağıdaki ürün bazında stok devir hızı verilerini analiz edip sadece ama sadece Türkçe yorum yap:
                {JsonSerializer.Serialize(data)}
                Ürünlerin stok devir hızlarını karşılaştır ve yüksek/düşük devir hızına sahip ürünlere dikkat çek.
                Stok yönetimi verimliliği hakkında değerlendirme yap ve iyileştirme için stratejik öneriler sun.
                Şirketin stok yönetim performansını profesyonel bir bakış açısıyla değerlendir.
                Kısa, öz ve profesyonel bir şekilde analiz yap, maksimum 3 paragraf.
                """,
                // Diğer grafik türleri için promptlar...
                _ => $"Veri analizi: {JsonSerializer.Serialize(data)}"
            };
        }
    }

    public record AnalysisRequest(string ChartType, object Data);
}