using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace ElasticSearchWith_ECommerce.Extensions
{
    public  static class ElasticSearchExt
    {
        public static void AddElastic(this IServiceCollection services, IConfiguration configuration) 
        {
            var username = configuration.GetSection("Elastic")["Username"];
            var password = configuration.GetSection("Elastic")["Password"];

            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elastic")["Url"])).Authentication(new BasicAuthentication(username, password));

            var client = new ElasticsearchClient(settings);

            services.AddSingleton(client);

        }
    }
}
