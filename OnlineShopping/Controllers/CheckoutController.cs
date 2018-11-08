using OnlineShopping;
using OnlineShopping.Controllers;
using OnlineShopping.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectricsOnlineWebApp.Controllers
{
    public class CheckoutController : BaseController
    {
        // GET: Checkout for testing
        public ActionResult Index()
        {
            ViewBag.Cart = _ctx.ShoppingCartDatas.ToList<ShoppingCartData>();
            return View();
        }
        public JsonResult QuanityChange(int type, int pId)
        {
            ProductEntities context = new ProductEntities();

            ShoppingCartData product = context.ShoppingCartDatas.FirstOrDefault(p => p.PID == pId);
            if (product == null)
            {
                return Json(new { d = "0" });
            }

            Product actualProduct = context.Products.FirstOrDefault(p => p.PID == pId);
            int quantity;
            // if type 0, decrease quantity
            // if type 1, increase quanity
            switch (type)
            {
                case 0:
                    product.Quantity--;
                    actualProduct.UnitsInStock++;
                    break;
                case 1:
                    product.Quantity++;
                    actualProduct.UnitsInStock--;
                    break;
                case -1:
                    actualProduct.UnitsInStock += product.Quantity;
                    product.Quantity = 0;
                    break;
                default:
                    return Json(new { d = "0" });
            }

            if (product.Quantity == 0)
            {
                context.ShoppingCartDatas.Remove(product);
                quantity = 0;
            }
            else
            {
                quantity = product.Quantity;
            }
            context.SaveChanges();
            return Json(new { d = quantity });
        }

        [HttpGet]
        public JsonResult UpdateTotal()
        {
            ProductEntities context = new ProductEntities();
            decimal total;
            try
            {
                total = context.ShoppingCartDatas.Select(p => p.UnitPrice * p.Quantity).Sum();
            }
            catch (Exception) { total = 0; }

            return Json(new { d = String.Format("{0:c}", total) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Clear()
        {
            try
            {
                List<ShoppingCartData> carts = _ctx.ShoppingCartDatas.ToList();
                carts.ForEach(a => {
                    Product product = _ctx.Products.FirstOrDefault(p => p.PID == a.PID);
                    product.UnitsInStock += a.Quantity;
                });
                _ctx.ShoppingCartDatas.RemoveRange(carts);
                _ctx.SaveChanges();
            }
            catch (Exception) { }
            return RedirectToAction("Index", "Home", null);
        }

        public ActionResult LoginCheck()
        {
            if (Session["User"] != null)
            {              
                return RedirectToAction("Purchase", "Checkout");
            }
            else
            {
                return RedirectToAction("LogIn", "Account");
            }
        }

        public ActionResult Purchase()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Purchase(Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    Order o = new Order
                    {
                        OrderDate = DateTime.Now,
                        DeliveryDate = DateTime.Now.AddDays(5),
                        CID = customer.CID
                    };

                    _ctx.Customers.Add(customer);
                    _ctx.Orders.Add(o);

                    foreach (var i in _ctx.ShoppingCartDatas.ToList<ShoppingCartData>())
                    {
                        _ctx.Order_Products.Add(new Order_Products
                        {
                            OrderID = o.OrderID,
                            PID = i.PID,
                            Qty = i.Quantity,
                            TotalSale = i.Quantity * i.UnitPrice
                        });
                        _ctx.ShoppingCartDatas.Remove(i);
                    }
                    _ctx.SaveChanges();
                    return RedirectToAction("PurchasedSuccess", "Checkout", o);
                }
            }

            List<ModelError> errors = new List<ModelError>();
            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    errors.Add(error);
                }
            }
            return View(customer);
        }
        public ActionResult PurchasedSuccess(Order o)
        {
            string email = Session["User"].ToString();

            var RegisterViewModel = (from reg in _ctx.Registrations
                                     join city in _ctx.Cities
                                     on reg.City equals city.CID
                                     where reg.Email == email
                                     select new RegisterCityViewModel
                                     {
                                         Register = reg,
                                         City = city
                                     }).ToList();

            var CustomerFilter = _ctx.Customers.FirstOrDefault(c => c.CID == o.CID);

            var OrderViewModel = (from or in _ctx.Orders
                                  join p in _ctx.Order_Products
                                  on or.OrderID equals p.OrderID into g
                                  where or.OrderID == o.OrderID
                                  select new OrderViewModel
                                  {
                                      Order = or,
                                      TotalPrice = g.Sum( x => x.TotalSale)
                                  }).ToList();

            ViewBag.Register = RegisterViewModel;
            ViewBag.Customer = CustomerFilter;
            ViewBag.Order = OrderViewModel;
            return View();
        }

    }
}
