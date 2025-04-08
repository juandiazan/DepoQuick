using Controladores;
using LogicaNegocio;
using Dominio;
using Repositorio;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<ContextoSql>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

//Scoped
builder.Services.AddScoped<IRepositorioFind<Usuario>,RepositorioUsuarioBaseDeDatos>();
builder.Services.AddScoped<IRepositorioCRUDFind<Promocion>, RepositorioPromocionBaseDeDatos>();
builder.Services.AddScoped<IRepositorioFindDelete<Deposito>, RepositorioDepositoBaseDeDatos>();
builder.Services.AddScoped<IRepositorioUpdate<Reserva>, RepositorioReservaBaseDeDatos>();

builder.Services.AddScoped<LogicaUsuario>();
builder.Services.AddScoped<LogicaPromocion>();
builder.Services.AddScoped<LogicaDeposito>();
builder.Services.AddScoped<LogicaReserva>();

builder.Services.AddScoped<ControladorSesion>();
builder.Services.AddScoped<ControladorUsuario>();
builder.Services.AddScoped<ControladorPromocion>();
builder.Services.AddScoped<ControladorDeposito>();
builder.Services.AddScoped<ControladorReserva>();
builder.Services.AddScoped<ControladorCalculoPrecio>();

//Singleton
//builder.Services.AddSingleton<IRepositorioFind<Usuario>, RepositorioUsuario>();
//builder.Services.AddSingleton<IRepositorio<Promocion>, RepositorioPromocion>();
//builder.Services.AddSingleton<IRepositorioFindDelete<Deposito>, RepositorioDeposito>();
//builder.Services.AddSingleton<IRepositorioBase<Reserva>, RepositorioReserva>();

builder.Services.AddSingleton<IExportadorReporte<Reserva>, ExportadorTxt>();
builder.Services.AddSingleton<IExportadorReporte<Reserva>, ExportadorCsv>();

builder.Services.AddSingleton<LogicaSesion>();
builder.Services.AddSingleton<LogicaCalculoPrecio>();

//builder.Services.AddSingleton<LogicaUsuario>();
//builder.Services.AddSingleton<LogicaPromocion>();
//builder.Services.AddSingleton<LogicaDeposito>();
//builder.Services.AddSingleton<LogicaReserva>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();