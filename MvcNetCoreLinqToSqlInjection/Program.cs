using MvcNetCoreLinqToSqlInjection.Models;
using MvcNetCoreLinqToSqlInjection.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//otra forma de añadir un coche
Coche car = new Coche();
car.Marca = "FORD";
car.Modelo = "MUSTANG DARKHORSE";
car.Imagen = "darkhorse.jpg";
car.Velocidad = 0;
car.VelocidadMaxima = 250;
builder.Services.AddSingleton<ICoche, Coche>(x => car);

//Resolvemos el Servicio Coche para la inyeccion
//builder.Services.AddTransient<Coche>(); los transients devuelven objeto nuevo al cargar, no sirve para forms pq lo pone a 0
//builder.Services.AddSingleton<Coche>();
//builder.Services.AddSingleton<Deportivo>();
//builder.Services.AddSingleton<ICoche, Deportivo>();

//Resolvemos los doctores, los repos suelen ir como transient
//builder.Services.AddTransient<RepositoryDoctoresSQLServer>();
//builder.Services.AddTransient<RepositoryDoctoresOracle>();
builder.Services.AddTransient<IRepositoryDoctores, RepositoryDoctoresOracle>();
//builder.Services.AddTransient<IRepositoryDoctores, RepositoryDoctoresSQLServer>();

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
    pattern: "{controller=Home}/{action=Index}/{id?}", "{controller=Doctores}/{action=Index}/{idDoctor?}")
    .WithStaticAssets();


app.Run();
