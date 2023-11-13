using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearchWith_ECommerce.Interfaces;
using ElasticSearchWith_ECommerce.Models;
using ElasticSearchWith_ECommerce.ViewModels;

namespace ElasticSearchWith_ECommerce.Repository
{
    public class ECommerceRepository : IECommerceRepository
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        private const string indexName = "kibana_sample_data_ecommerce";

        public ECommerceRepository(ElasticsearchClient elasticSearchClient)
        {
            _elasticsearchClient = elasticSearchClient;
        }

        public async Task<(List<ECommerce> eCommerceList, long totalCount)> SearchAsync(ECommerceSearchViewModel searchViewModel, int page, int pageSize)
        { //Yukarda iki değer döndük birinde ECommerce datalarını list şeklinde, diğerini de sayfalama(pagination) için count

            //Total Count 100 olsun
            //page = 1 pageSize = 10 olduğunda 1-10 arasındaki dataları döner
            //page = 2 pageSize = 10 olduğunda 11-20 arasındaki dataları döner

            
            List<Action<QueryDescriptor<ECommerce>>> listQuery = new();



            if (!string.IsNullOrEmpty(searchViewModel.Category))
            {
                Action<QueryDescriptor<ECommerce>> query = q => q.Match(m => m
                                                                 .Field(f => f.Category)
                                                                 .Query(searchViewModel.Category));
                listQuery.Add(query); //Sorgulamayı yaptık ve gelen datayı listQuery'nin içerisine attıkk.


            }

            if (!string.IsNullOrEmpty(searchViewModel.CustomerFullName))
            {
                Action<QueryDescriptor<ECommerce>> query = q => q.Match(m => m
                                                                 .Field(f => f.CustomerFullName)
                                                                 .Query(searchViewModel.CustomerFullName));

                listQuery.Add(query);
            }

            if (searchViewModel.OrderDateStart.HasValue) //HasValue o fielda(searchViewModel.OrderDateStart) değer geliyomu gelmiyo mu ona bakar geliyosa true döner, gelmiyosa false döner
            {
                Action<QueryDescriptor<ECommerce>> query = q => q.Range(r => r
                                                                 .DateRange(dr => dr
                                                                 .Field(f => f.OrderDate)
                                                                 .Gte(searchViewModel.OrderDateStart.Value)));

                listQuery.Add(query);
            }

            if (searchViewModel.OrderDateEnd.HasValue) //HasValue filed'ın içi doluysa true, boşsa false döner
            {
                Action<QueryDescriptor<ECommerce>> query = q => q.Range(r => r
                                                                 .DateRange(dr => dr
                                                                 .Field(f => f.OrderDate)
                                                                 .Lte(searchViewModel.OrderDateEnd.Value)));
                listQuery.Add(query);
            }

            if (!string.IsNullOrEmpty(searchViewModel.Gender))
            {
                Action<QueryDescriptor<ECommerce>> query = q => q.Term(t => t
                                                                 .Field(f => f.Gender)
                                                                 .Value(searchViewModel.Gender));
                listQuery.Add(query);
            }

            var pageFrom = (page - 1) * pageSize;

            var response = await _elasticsearchClient.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Size(pageSize)
            .From(pageFrom)
            .Query(q => q
            .Bool(b => b
            .Must(listQuery.ToArray()))));

            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return (response.Documents.ToList(), response.Total);


        }


    }
}
