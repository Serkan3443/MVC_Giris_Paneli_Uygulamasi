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
    new CultureInfo("en-US")//e�er uygulama T�rk�e olacaksa CultureInfo constructor`�n�n parametresini("tr-TR) yapmak yeterlidir
};
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name);
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;

});
#endregion
#region Ioc Container
//ba��ml�lk y�netimi
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

app.UseHttpsRedirection();//https protokol� 
app.UseStaticFiles();//wwwroot ->html,css,js,imaj,video,m�zik vb. dosyalar

app.UseRouting();

app.UseAuthorization();//yetki kontrol�

app.MapControllerRoute(//web uygulamas�n�n varsay�lan route(yol) tan�m�
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
