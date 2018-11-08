using OnlineShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopping.ViewModels
{
    public class NewCityViewModel
    {
        public IEnumerable<City> ListofCity { get; set; }
        public Register Register { get; set; }
    }
}