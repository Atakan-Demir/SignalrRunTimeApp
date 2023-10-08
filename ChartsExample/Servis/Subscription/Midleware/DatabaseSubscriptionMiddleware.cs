using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Servis.Subscription.Midleware
{
    static public class DatabaseSubscriptionMiddleware
    {
        // middleware lerin 'use' ile başlaması bir gelenektir
        // uygulama ayağa kalkmadan önce yapılması gereken işlemler
        
        public static void UseDatabeseSubscription<T>(this IApplicationBuilder builder, string tableName) where T : class, IDatabaseSubscription
        {
            // Servisi al ve Configure metodunu çağır
            var subscription = builder.ApplicationServices.GetService(typeof(T)) as T;
            if (subscription != null)
                subscription.Configure(tableName);
        }
    }
}
