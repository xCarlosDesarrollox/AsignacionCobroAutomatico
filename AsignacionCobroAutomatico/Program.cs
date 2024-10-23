using AsignacionCobroAutomatico.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRepositorioClientes, RepositorioClientes>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<IRepositorioPagoAutomatizado, RepositorioPagoAutomatizado>();
builder.Services.AddTransient<IRepositorioClientes, RepositorioClientes>();
builder.Services.AddTransient<IRepositorioServiciosFijos, RepositorioServiciosFijos>();
builder.Services.AddTransient<IRepositorioTarjeta, RepositorioTarjeta>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Clientes}/{action=Index}/{id?}");

app.Run();
