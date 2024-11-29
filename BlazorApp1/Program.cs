using BlazorApp1.Components;
using System.Net.Http;
using TallerModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<TurnoServices>();
builder.Services.AddScoped<MecanicoServices>();
builder.Services.AddScoped<UsuarioServices>();

// Agregar HttpClient como un servicio para Blazor Server
builder.Services.AddHttpClient();

// Agregar servicios para Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configuración del pipeline de manejo de errores
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

// Mapeo de Razor Components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();