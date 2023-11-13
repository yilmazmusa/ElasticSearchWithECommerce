using System.Text.Json.Serialization;

namespace ElasticSearchWith_ECommerce.ViewModels
{
    public class ECommerceViewModel // Bu ViewModel ECommerceService'ten gelen dataları UI tarafına dönmek için.YANİ BURASI ARTIK UI a BASILACAK DATA
    {
        public string Id { get; set; } = null!;

        public string CustomerFirstName { get; set; } = null!;

        public string CustomerLastName { get; set; } = null!;

        public string CustomerFullName { get; set; } = null!;

        public string Gender { get; set; }

        public string Category { get; set; } = null!;

        public int OrderId { get; set; }

        public string OrderDate { get; set; } //Normalde tipi DateTime'dı ben string yaptım çünkü burası(ECommerceViewModel) UI a basılacak data istediğim gibi basarım


        public double Taxful_Total_Price { get; set; }


    }



}
