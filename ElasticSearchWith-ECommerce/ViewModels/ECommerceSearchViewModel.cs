﻿using System.ComponentModel.DataAnnotations;

namespace ElasticSearchWith_ECommerce.ViewModels
{
    public class ECommerceSearchViewModel // Bu ViewModel UI'dan  gelen request bilgilerini karşılayabilmek için
    {
        [Display(Name = "Customer Full Name")]
        public string? CustomerFullName { get; set; }

        [Display(Name = "Category")]
        public  string? Category { get; set; }

        [Display(Name = "Cinsiyet")]
        public string? Gender { get; set; }

        [Display(Name = "Order Date (Start)")]
        [DataType(DataType.Date)] //Sadece tarihi alsın saati almasın dedik.Yani 13.11.2023 şeklinde 
        public DateTime? OrderDateStart { get; set; }

        [Display(Name = "Order Date (End)")]
        [DataType(DataType.Date)]
        public DateTime? OrderDateEnd { get; set; }
        

    }
}
