using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using MVC.context;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

#region Localization
List<CultureInfo> cultures = new List<CultureInfo>()
{
    new CultureInfo("en-US")//eðer uygulama Türkçe olacaksa CultureInfo constructor`ýnýn parametresini("tr-TR) yapmak yeterlidir
};
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name);
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;

});
#endregion
#region Ioc Container
//baðýmlýlk yönetimi
//Autofac, Ninject
builder.Services.AddDbContext<Db>(options => options.UseSqlServer(connectionString));
#endregion

builder.Services.AddControllersWithViews();

var app = builder.Build();

#region Localization
app.UseRequestLocalization(new RequestLocalizationOptions(){
    DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name),
    SupportedCultures=cultures,
    SupportedUICultures=cultures,
});
#endregion
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();//https protokolü 
app.UseStaticFiles();//wwwroot ->html,css,js,imaj,video,müzik vb. dosyalar

app.UseRouting();

app.UseAuthorization();//yetki kontrolü

app.MapControllerRoute(//web uygulamasýnýn varsayýlan route(yol) tanýmý
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
