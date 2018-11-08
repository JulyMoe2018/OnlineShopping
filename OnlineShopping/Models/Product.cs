using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopping
{
    public partial class Product
    {
        private ProductEntities _ctx = new ProductEntities();
        public List<Product> All
        {
            get
            {
                return _ctx.Products.ToList<Product>();
            }
        }

    }
}