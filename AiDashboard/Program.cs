using AiDashboard.Data;
using AiDashboard.Services;
using Microsoft.EntityFrameworkCore;

// Uygulama olu�turucuyu ba�lat
var builder = WebApplication.CreateBuilder(args);

// Servisleri containera ekle (B�T�N SERV�S KAYITLARI BURADA OLMALI)
builder.Services.AddControllersWithViews();

// PostgreSQL i�in DbContext ekle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// HttpClient servislerini ekle (OllamaService i�in)
builder.Services.AddHttpClient(); // Genel HttpClient
builder.Services.AddHttpClient<OllamaService>(); // �zel tip HttpClient
builder.Services.AddScoped<OllamaService>(); // Servisin kendisini kaydet

// UYGULAMAYI OLU�TUR (bundan sonra servis EKLENEMEZ)
var app = builder.Build();

// HTTP istek pipeline'�n� yap�land�r
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Route yap�land�rmas�
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=FinansalVeriler}/{id?}");

app.Run();