using ElasticSearchWith_ECommerce.Interfaces;
using ElasticSearchWith_ECommerce.Models;
using ElasticSearchWith_ECommerce.ViewModels;

namespace ElasticSearchWith_ECommerce.Services
{
    public class ECommerceService : IECommerceService
    {
        private readonly IECommerceRepository _repository;

        public ECommerceService(IECommerceRepository repository)
        {
            _repository = repository;
        }

        public async Task<(List<ECommerceViewModel> eCommerceListViewModel, long totalCount, long pageLinkCount)> SearchAsync(ECommerceSearchViewModel searchViewModel, int page, int pageSize)
        {
            try
            {
                //list dönücez,
                //total count dönücez,
                //sayfalama datası göstericem(1 2 3 4 5 gibi sayfalama).Mesela 46 kayıt varsa
                //1 => 1-10, 2 => 11-20, 3 => 21-30, 4 => 31-40, 5 => 41-46

                var (eCommerceList, totalCount) = await _repository.SearchAsync(searchViewModel, page, pageSize);

                var pageLinkCountCalculate = totalCount % pageSize;
                long pageLinkCount = 0;

                if (pageLinkCountCalculate == 0) // Tam bölünüyorsa mesela 60 / 10 dan kalan 0 dır o zaman 60 data için 6 page olucak
                {
                    pageLinkCount = totalCount / pageSize; // Tam bölünüyo
                }
                else                            // Tam bölünmüyorsa mesela 66 / 10 dan kalan 6 dır o zaman 60 data için 6 + 1 = 7  page olucak
                {
                    pageLinkCount = (totalCount / pageSize) + 1;
                }

                var eCommerceListViewModel = eCommerceList.Select(s => new ECommerceViewModel() // Bize ECommerceRepository'den ECommerce tipinde liste şeklinde olan datanın tipini
                                                                                                //ECommerceViewModel'e çevirip UI da bu ViewModel ile gösterdik.
                                                                                                //Direk ECommerce tipinde göstermek riskli
                {
                    Id = s.Id,
                    CustomerFirstName = s.CustomerFirstName,
                    CustomerLastName = s.CustomerLastName,
                    CustomerFullName = s.CustomerFullName,
                    Gender = s.Gender.ToUpper(),
                    Category = string.Join(",", s.Category), // , ler ayrılmış dataları (ben,seni,çok,sevdim i aynı şekilde ben,seni,çok,sevdim olarak getir diyoruz)
                    OrderId = s.OrderId,
                    OrderDate = s.OrderDate.ToShortDateString(),// Kısa tarih şeklinde string'e çevirip bassın dedim çünkü ECommerceViewModel de OrderDate'in formatı string
                    Taxful_Total_Price = s.Taxful_Total_Price,
                }).ToList();

                return (eCommerceListViewModel, totalCount, pageLinkCount);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

    }
}
