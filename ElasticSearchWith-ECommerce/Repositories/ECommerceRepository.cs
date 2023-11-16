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


            if (searchViewModel is null)  //searchViewModel'in boş gelme durumuna göre tüm datayı dönüyoruz
            {
                Action<QueryDescriptor<ECommerce>> query = q => q.MatchAll();

                listQuery.Add(query);

                return await CalculateResultPage(page, pageSize, listQuery); //searchViewModel boş ise tüm datayı çek
                                                                             //sonra o datayı(listQuery) requestte gelen page
                                                                             //ve pageSize değerlerine göre UI da dön
                                                                             //alttaki if lere boş yere bakma dedik.
            }



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
                                                                 .Value(searchViewModel.Gender).CaseInsensitive());
                listQuery.Add(query);
            }

            if (!listQuery.Any()) // Yukardaki if lerin hiç birine girmezse listQuery lsitesinin içi boş kalır
                                  // içine data eklemsi olmaz, boş olursada aşağıda patlar(99.satır)
            {
                Action<QueryDescriptor<ECommerce>> query = q => q.MatchAll();
                listQuery.Add(query);
            }

            return await CalculateResultPage(page, pageSize, listQuery); //searchViewModel boş değilse yukardaki if lerden birilerine ya da hepsine girer
                                                                         //sogulamalar sonucunda gelen datalarla(listQuery), requestte gelen page ve pageSize
                                                                         //değerlerine göre UI da döner.

        }

        private async Task<(List<ECommerce> eCommerceList, long totalCount)> CalculateResultPage(int page, int pageSize, List<Action<QueryDescriptor<ECommerce>>> listQuery)
        {
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
