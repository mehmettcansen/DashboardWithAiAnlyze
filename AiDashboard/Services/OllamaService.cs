using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AiDashboard.Services
{
    public class OllamaService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OllamaService> _logger;
        private const string BaseUrl = "http://localhost:11434/api/";

        public OllamaService(HttpClient httpClient, ILogger<OllamaService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<string> GenerateAnalysisAsync(string prompt)
        {
            try
            {
                var requestData = new
                {
                    model = "llama3",
                    prompt,
                    stream = false
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestData),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("generate", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(responseContent);
                return jsonDoc.RootElement.GetProperty("response").GetString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ollama API hatası");
                return "Analiz oluşturulurken hata oluştu";
            }
        }
    }
}