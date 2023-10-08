using Microsoft.AspNetCore.SignalR;
using Servis.Hubs;
using Servis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TableDependency.SqlClient;

namespace Servis.Subscription
{

    public interface IDatabaseSubscription
    {
        // veritabanındaki tablo isimlerini alabilmesi için
        void Configure(string tableName);
    }                                                           // T 'yi nesne türetilebilir bir parametreye dönüştürdük
    public class DatabaseSubscription<T> : IDatabaseSubscription where T : class, new()
    {
        // Connection Str
        IConfiguration _configuration;
        // Hubın değişiklikler yakalandığında işlem yapabilmesi için hub Contextimizi çağırıyoruz
        IHubContext<SatisHub> _hubContext;
        public DatabaseSubscription(IConfiguration configuration, IHubContext<SatisHub> hubContext)
        {
            _configuration = configuration;
            _hubContext = hubContext;
        }


        SqlTableDependency<T> _tableDependency;
        public void Configure(string tableName)
        {
            _tableDependency = new SqlTableDependency<T>(_configuration.GetConnectionString("SQL"), tableName);
            _tableDependency.OnChanged += async (o, e) =>
            {
                

                SatisDbContext contex = new SatisDbContext();
                // sql'e gidecek sorgu
                var data = (from personel in contex.Personellers
                            join satis in contex.Satislars
                            on personel.Id equals satis.PersonelId
                            select new { personel, satis }).ToList();

                List<object> datas = new List<object>();
                var personelİsimleri = data.Select(d => d.personel.Adi).Distinct().ToList();

                personelİsimleri.ForEach(p =>
                {
                    datas.Add(new
                    {
                        name = p,
                        data = data.Where(q => q.personel.Adi == p).Select(s => s.satis.Fiyat).ToList()
                    });
                });

                await _hubContext.Clients.All.SendAsync("reciveMessage", datas);

            };
            _tableDependency.OnError += (o, e) =>
            {

            };
            _tableDependency.Start();
        }

        //deconstructor : nesne imha edilirken son kez 'haydi selametle' dememizi sağlayan fonksiyon
        ~DatabaseSubscription()
        {
            _tableDependency.Stop();
        }
    }
}
