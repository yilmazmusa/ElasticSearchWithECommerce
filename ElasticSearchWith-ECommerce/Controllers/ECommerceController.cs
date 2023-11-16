using ElasticSearchWith_ECommerce.Interfaces;
using ElasticSearchWith_ECommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearchWith_ECommerce.Controllers
{
    public class ECommerceController : Controller
    {
        private readonly IECommerceService _service; //Neden readonly tanımlarız çünkü ya ilk yazıldığı
                                                     //yerde ya da constructor da inişilayz ederiz başka yerde etmeye izin vermez.
                                                     //İzin vermemesi iyi bir şey çünkü biri gelir _service' e yeni bir service tanımlaması yapmasın diye

        public ECommerceController(IECommerceService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Search([FromQuery] SearchPageViewModel searchPageViewModel)
        {



            var (eCommerceListViewModel, totalCount, pageLinkCount) = await _service.SearchAsync(searchPageViewModel.SearchViewModel,searchPageViewModel.Page,searchPageViewModel.PageSize);
            //ECommerceService'ten bize bu datalar geliyor, aynı isimde olması zorunlu değil ben anlaması kolay olsun diye ECommerceServiceteki isimlerin aynısını verdim.

            searchPageViewModel.ECommerceViewModelList = eCommerceListViewModel;
            searchPageViewModel.TotalCount = totalCount;
            searchPageViewModel.PageLinkCount = pageLinkCount;

            return View(searchPageViewModel);
        }
    }
}
