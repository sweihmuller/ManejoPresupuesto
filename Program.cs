using ManejoPresupuesto.Servicios;
using AutoMapper;
using ManejoPresupuesto.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRepositorioTipoCuentas, RepositorioTipoCuentas>();
builder.Services.AddTransient<IRepositorioCuentas, RepositorioCuentas>();
builder.Services.AddTransient<IRepositorioCategorias, RepositorioCategorias>();
builder.Services.AddTransient<IRepositorioTransacciones, RepositorioTransacciones>();
builder.Services.AddTransient<IServicioUsario, ServicioUsuario>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IServiciosReporte, ServicioReporte>();
builder.Services.AddAutoMapper((cfg) =>
{
    cfg.CreateMap<Transaccion, TransaccionCreacionViewModel>();
    cfg.CreateMap<TransaccionActualizacionViewModel, Transaccion>().ReverseMap();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Transacciones}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
