using Servis.Hubs;
using Servis.Models;
using Servis.Subscription;
using Servis.Subscription.Midleware;

namespace Servis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services);
            var app = builder.Build();
            Configure(app);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            //Cors politikalar� : Taray�c�lar�n API g�venliklerini sa�lamak i�in alm�� oldu�u varsay�lan g�venlik �nlemleridir.
            //geli�tirme a�amas�nda a�a��daki gibi, cors politikalar� zay�flat�lm��t�r
            services.AddCors(options => options.AddDefaultPolicy(policy =>
            policy.AllowCredentials().AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(x => true)));

            // SignalR k�t�phanesini dahil edelim
            services.AddSignalR();
            // services.AddSingleton ile bir servisi kaydediyoruz
            services.AddSingleton<DatabaseSubscription<Satislar>>();
            services.AddSingleton<DatabaseSubscription<Personeller>>();
            // yukar�daki iki tabloda dinlenilecek
        }
        
        private static void Configure(WebApplication app)
        {
            // ayarlanm�� cors politikalar�n� kullan�yoruz
            app.UseCors();

            app.UseDatabeseSubscription<DatabaseSubscription<Satislar>>("Satislar");
            app.UseDatabeseSubscription<DatabaseSubscription<Personeller>>("Personeller");

            app.UseRouting();
            // endpointleri ayarl�yoruz
            app.UseEndpoints(endpoints =>
            {
                //hub i�in
                endpoints.MapHub<SatisHub>("/satishub");
            });
        }
    }
}