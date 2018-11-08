using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopping.Models
{
    public class Customer
    {
        public int CID { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Required(ErrorMessage = "LastNme is required")]
        [Display(Name = "Last Name")]
        public string LName { get; set; }
     
        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Display(Name = "Telphone")]
        [Required(ErrorMessage = "Telphone is required")]
        public string Telephone { get; set; }
    }
}