using Microsoft.AspNetCore.SignalR;
using Servis.Hubs;
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
    public class DatabaseSubscription<T> : IDatabaseSubscription where T : class,new()
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
                await _hubContext.Clients.All.SendAsync("reciveMessage", "Merhaba");
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
