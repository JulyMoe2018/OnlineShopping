using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopping.ViewModels
{
    public class OrderViewModel
    {
        public Order Order {get; set;}
        public Order_Products Order_Product { get; set; }

        public decimal TotalPrice { get; set; }

    }
}