using ElasticSearchWith_ECommerce.Models;
using ElasticSearchWith_ECommerce.ViewModels;

namespace ElasticSearchWith_ECommerce
{
    public interface IECommerceRepository
    {
        public  Task<(List<ECommerce> list, long count)> SearchAsync(ECommerceSearchViewModel searchViewModel, int page, int pageSize);
    }
}
