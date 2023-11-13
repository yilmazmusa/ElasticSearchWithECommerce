using ElasticSearchWith_ECommerce.ViewModels;

namespace ElasticSearchWith_ECommerce.Interfaces
{
    public interface IECommerceService
    {
        public Task<(List<ECommerceViewModel> eCommerceListViewModel, long totalCount, long pageLinkCount)> SearchAsync(ECommerceSearchViewModel searchViewModel, int page, int pageSize);
    }
}
