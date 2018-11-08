using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopping.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home for testing
        public ActionResult Index()
        {
            List<Product> products = _ctx.Products.Where(p => p.Deleted == false).ToList<Product>();
      
            ViewBag.Products = products;         
            return View();
        }
        public ActionResult Index3()
        {
            List<Product> products = _ctx.Products.Where(p => p.Deleted == false).ToList<Product>();

            ViewBag.Products = products;
            return View();
        }

        public ActionResult Details(int productId)
        {
            List<Product> productDetails = _ctx.Products.Where(p => p.PID == productId).ToList<Product>();
            ViewBag.productDetails = productDetails;
            return View();
        }
        public ActionResult Category(string catName)
        {
            List<Product> products;
            if (catName == "")
            {
                products = _ctx.Products.ToList<Product>();
            }
            else
            {
                products = _ctx.Products.Where(p => p.Category == catName).ToList<Product>();
            }
            ViewBag.Products = products;
            return View("Index");
        }

        public ActionResult Discount(int? disName)
        {
            List<Product> products;
            if (disName == null)
            {
                products = _ctx.Products.ToList<Product>();
            }
            else
            {
                products = _ctx.Products.Where(p => p.Discount == disName).ToList<Product>();
            }
            ViewBag.Products = products;
            return View("Index");
        }

        public ActionResult Suppliers()
        {
            List<Supplier> suppliers = _ctx.Suppliers.ToList<Supplier>();
            ViewBag.Suppliers = suppliers;
            return View();
        }

        public ActionResult AddToCart(int id)
        {
            addToCart(id);
            return RedirectToAction("Index");
        }

        private void addToCart(int pId)
        {
            // check if product is valid
            Product product = _ctx.Products.FirstOrDefault(p => p.PID == pId);
            if (product != null && product.UnitsInStock > 0)
            {
                // check if product already existed
                ShoppingCartData cart = _ctx.ShoppingCartDatas.FirstOrDefault(c => c.PID == pId);
                if (cart != null)
                {
                    cart.Quantity++;
                }
                else
                {
                    cart = new ShoppingCartData
                    {
                        PName = product.PName,
                        PID = product.PID,
                        UnitPrice = product.UnitPrice,
                        Quantity = 1
                    };

                    _ctx.ShoppingCartDatas.Add(cart);
                }
                product.UnitsInStock--;
                _ctx.SaveChanges();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Creation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creation(OnlineShopping.Models.CreateProduct product)
        {
            if (ModelState.IsValid)
            {
                int MaxContentLength = 1024 * 1024 * 3; //3 MB
                string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf" };
                string[] allowfile = new string[] { ".jpg", ".gif", ".png", ".pdf" };

                if (!AllowedFileExtensions.Contains(product.Photo.FileName.Substring(product.Photo.FileName.LastIndexOf('.'))))
                {
                    ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                }

                else if (product.Photo.ContentLength > MaxContentLength)
                {
                    ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                }

                else
                {
                    string path = Path.Combine(Server.MapPath("~/Photo/Products/"), Path.GetFileName(product.Photo.FileName));
                    product.Photo.SaveAs(path);

                    Product p = new Product
                    {
                        PName = product.PName,
                        UnitPrice = product.UnitPrice,
                        UnitsInStock = product.UnitsInStock,
                        Category = product.Category,
                        Photo = product.Photo.FileName,
                        Description = product.Description,
                        SID = product.SID,
                        Deleted = false,
                        Discount = product.Discount
                    };
                    _ctx.Products.Add(p);
                    _ctx.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                ModelState.AddModelError("", "Data is not correct");
            }

            List<ModelError> errors = new List<ModelError>();
            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    errors.Add(error);
                }
            }
            return View(product);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var pro = _ctx.Products.Where(p => p.PID == id).FirstOrDefault();
            return View(pro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OnlineShopping.Product product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                Product pro = _ctx.Products.Where(p => p.PID == product.PID).FirstOrDefault();

                if (pro == null)
                {
                    return HttpNotFound();
                }

                else
                {
                    if(file != null)
                    {
                        string path = Path.Combine(Server.MapPath("~/Photo/Products/"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        pro.Photo = file.FileName;
                    } 
                                      
                    pro.PName = product.PName;
                    pro.UnitPrice = product.UnitPrice;
                    pro.UnitsInStock = product.UnitsInStock;
                    pro.Category = product.Category;                   
                    pro.Description = product.Description;

                    _ctx.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                
            }
            else
            {
                ModelState.AddModelError("", "Data is not correct");
            }

            List<ModelError> errors = new List<ModelError>();
            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    errors.Add(error);
                }
            }

            return View(product);
        }
        public JsonResult DeleteProduct(int id)
        {
            bool result = false;
            var product = _ctx.Products.Where(p => p.PID == id).FirstOrDefault();
            if (product != null)
            {
                product.Deleted = true;
                _ctx.SaveChanges();
                
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult RateProduct(int id)
        {
            ViewBag.ProductId = id;
            return View();
        }

        public ActionResult Reviews(int id)
        {
            var reviews = _ctx.Feedbacks.Where(r => r.PId == id).ToList();
            ViewBag.Reviews = reviews;
            return View();

        }
        public JsonResult Feedback(int id, string feedback)
        {
            int result;
            if(Session["User"] != null)
            {
                Feedback p = new Feedback()
                {
                    Email = Session["User"].ToString(),
                    CreateTime = DateTime.Now,
                    Feedback1 = feedback,
                    PId = id
                };
                _ctx.Feedbacks.Add(p);
                _ctx.SaveChanges();

                result = 1;
            }
            else
            {
                result = 0;
            }
            
            return Json(result);
        }

    }
}