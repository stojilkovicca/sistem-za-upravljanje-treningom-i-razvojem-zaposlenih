using TreningIRazvoj.Frontend.Servisi;
var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IApiServis, ApiServis>();

// Omogućava čuvanje JWT tokena u sesiji
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(opcije =>
{
    opcije.IdleTimeout = TimeSpan.FromHours(2);
    opcije.Cookie.HttpOnly = true;
    opcije.Cookie.IsEssential = true;
});

// HttpClient koji poziva naš Web API
builder.Services.AddHttpClient(
    "TreningIRazvojAPI",
    klijent =>
    {
        var osnovnaAdresa = builder.Configuration[
            "ApiPodesavanja:OsnovnaAdresa"];

        if (string.IsNullOrWhiteSpace(osnovnaAdresa))
        {
            throw new InvalidOperationException(
                "Osnovna adresa API-ja nije podešena.");
        }

        klijent.BaseAddress = new Uri(osnovnaAdresa);
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Sesija mora biti uključena pre mapiranja kontrolera
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Autentifikacija}/{action=Prijava}/{id?}");

app.Run();