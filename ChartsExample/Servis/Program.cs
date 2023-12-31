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
            //Cors politikaları : Tarayıcıların API güvenliklerini sağlamak için almış olduğu varsayılan güvenlik önlemleridir.
            //geliştirme aşamasında aşağıdaki gibi, cors politikaları zayıflatılmıştır
            services.AddCors(options => options.AddDefaultPolicy(policy =>
            policy.AllowCredentials().AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(x => true)));

            // SignalR kütüphanesini dahil edelim
            services.AddSignalR();
            // services.AddSingleton ile bir servisi kaydediyoruz
            services.AddSingleton<DatabaseSubscription<Satislar>>();
            services.AddSingleton<DatabaseSubscription<Personeller>>();
            // yukarıdaki iki tabloda dinlenilecek
        }
        
        private static void Configure(WebApplication app)
        {
            // ayarlanmış cors politikalarını kullanıyoruz
            app.UseCors();

            app.UseDatabeseSubscription<DatabaseSubscription<Satislar>>("Satislar");
            app.UseDatabeseSubscription<DatabaseSubscription<Personeller>>("Personeller");

            app.UseRouting();
            // endpointleri ayarlıyoruz
            app.UseEndpoints(endpoints =>
            {
                //hub için
                endpoints.MapHub<SatisHub>("/satishub");
            });
        }
    }
}