using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShopping.Models
{
    public class CreateProduct
    {
        public int PID { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        [Display(Name = "Product Name: ")]
        public string PName { get; set; }

        [Required(ErrorMessage = "Product Price is required")]
        [Display(Name = "Product Price: ")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Unit in stock is required")]
        [Display(Name = "Units In Stock")]
        public int UnitsInStock { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Please Upload File")]
        [Display(Name = "Upload File")]
        public HttpPostedFileBase Photo { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Supplier Name")]
        public Nullable<int> SID { get; set; }

        [Display(Name = "Discout")]
        public Nullable<int> Discount { get; set; }

    }
}