using ERP_Consumer.Helpers;
using ERP_Consumer.Services;
using ERP_Consumer.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ── Options ──────────────────────────────────────────────────────────────────
builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection(ApiSettings.SectionName));

var baseUrl = builder.Configuration[$"{ApiSettings.SectionName}:BaseUrl"]
              ?? throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");

// ── HttpClient + Services ────────────────────────────────────────────────────
builder.Services.AddHttpClient<ICustomerApiService, CustomerApiService>(c =>
    c.BaseAddress = new Uri(baseUrl));

builder.Services.AddHttpClient<IVehicleApiService, VehicleApiService>(c =>
    c.BaseAddress = new Uri(baseUrl));

builder.Services.AddHttpClient<ICategoryApiService, CategoryApiService>(c =>
    c.BaseAddress = new Uri(baseUrl));

builder.Services.AddHttpClient<IServiceApiService, ServiceApiService>(c =>
    c.BaseAddress = new Uri(baseUrl));

builder.Services.AddHttpClient<IPartApiService, PartApiService>(c =>
    c.BaseAddress = new Uri(baseUrl));

// ── MVC ───────────────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
