using ElasticSearchWith_ECommerce.Models;
using ElasticSearchWith_ECommerce.ViewModels;

namespace ElasticSearchWith_ECommerce.Interfaces
{
    public interface IECommerceRepository
    {
        public Task<(List<ECommerce> eCommerceList, long totalCount)> SearchAsync(ECommerceSearchViewModel searchViewModel, int page, int pageSize);
    }
}
