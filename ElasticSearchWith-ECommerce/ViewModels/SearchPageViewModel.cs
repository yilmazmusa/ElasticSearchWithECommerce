namespace ElasticSearchWith_ECommerce.ViewModels
{
    public class SearchPageViewModel
    {
        public  long TotalCount { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public long  PageLinkCount { get; set; }
        public List<ECommerceViewModel> ECommerceViewModelList { get; set; }
        public ECommerceSearchViewModel SearchViewModel { get; set; }


        public int StartPage()
        {
            return (Page - 5) <= 0 ? 1 : (Page - 5);
        }
        
        public long EndPage()
        {
            return (Page + 5) >= PageLinkCount ? PageLinkCount : (Page + 5);
        }


        public string CreatePageUrl(HttpRequest request, long page, int pageSize)  // Dinamik Url oluşturan metod
        {
            //https://www/search?querystringdegeri

            var currentUrl = new Uri($"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}").AbsolutePath;
            //Yukarda gelen istekteki Url deki bilgileri komple aldık.AbsolutePath diyerek tam path'i al dedik.

            if (currentUrl.Contains("page",StringComparison.OrdinalIgnoreCase)) //Url içerisinde page ifadesi var mı diye baktık.StringComparison.OrdinalIgnoreCase dediğimiz için büyük/küçük harf duyarlılı olmadan arıyor.
            {
                currentUrl = currentUrl.Replace($"Page={Page}", $"Page={page}",StringComparison.OrdinalIgnoreCase);
                //Page in yerine requestte gelen page i koy dedik yani URL i sürekli güncelliyoruz
                //Client'ın isteğine göre çünkü adam bmw ye bakarken mercedes e bakmak isteyebilir.

                currentUrl = currentUrl.Replace($"PageSize={PageSize}", $"Page={pageSize}", StringComparison.OrdinalIgnoreCase);
                //Aynı şekilde PageSize'ı da Clientten gelen isteğe göre sürekli güncelliyoruz.
            }

            else
            {
                currentUrl = $"{currentUrl}?Page={page}";

                currentUrl = $"{currentUrl}&PageSize={pageSize}";
            }

            return currentUrl;
        }

    }
}
