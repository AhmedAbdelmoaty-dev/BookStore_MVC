using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using Models.Entites;
using Models.ViewModels;
using Utilites;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = StaticDetails.Role_Admin)]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = unitOfWork;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var ProductsList = _UnitOfWork.Product.GetAll(includeProperties: nameof(Product.Category));
            return View(ProductsList);
        }
        public IActionResult Upsert(int? id)
        {
            
            ProductVM productVM = new ProductVM()
            {
                CategoryList = _UnitOfWork.Category.GetAll().Select(
               temp => new SelectListItem { Text = temp.Name, Value = temp.ID.ToString() }
              ),
                product = new Product()

            };

            if (id == null||id==0)
            {
                return View(productVM);
            }
            else
            {
                productVM.product = _UnitOfWork.Product.Get(temp => temp.ID == id);
                return View(productVM);
            }
            
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM,IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            //place of wwwroot on machine like for example C:\Projects\MyApp
            string wwwRootPath = _WebHostEnvironment.WebRootPath;
            if (file != null)
            {
                //creates a random guid as a name for the file and append the extension 
                //of the file we got from form at the end of the guid
              
                string FileName=Guid.NewGuid().ToString() +Path.GetExtension(file.FileName);

                //it combines the root folder path on machine and the path of folder you mention
                //for example return will be C:\Projects\MyApp\wwwroot\images\product
                string ProductPath =Path.Combine(wwwRootPath,@"images\product");

                if (!string.IsNullOrEmpty(productVM.product.ImageUrl))
                {
                    var OldImagePath = Path.Combine(wwwRootPath, productVM.product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(OldImagePath))
                    {
                        System.IO.File.Delete(OldImagePath);
                    }
                }
                using (var FileStream = new FileStream(Path.Combine(ProductPath, FileName), FileMode.Create))
                {
                    file.CopyTo(FileStream);
                }
                productVM.product.ImageUrl = @"\Images\Product\" + FileName;
            }
            if (productVM.product.ID == 0 || productVM.product == null)
            {
                TempData["Success"] = "Product Added Succeccfully";
                _UnitOfWork.Product.Add(productVM.product);
            }
            else
            {
                TempData["Success"] = "Product Updated Succeccfully";
                _UnitOfWork.Product.Update(productVM.product);
            }
        
        
            _UnitOfWork.Save();
            return RedirectToAction("Index");

        }



        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? obj = _UnitOfWork.Product.Get(temp => temp.ID == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(obj);
        //}
        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeleteProduct(int? id)
        //{
        //    Product? obj = _UnitOfWork.Product.Get(temp => temp.ID == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _UnitOfWork.Product.Delete(obj);
        //    _UnitOfWork.Save();
        //    TempData["Success"] = "Category Deleted Succeccfully";
        //    return RedirectToAction("Index");
        //}
        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var ProductsList=_UnitOfWork.Product.GetAll(includeProperties: nameof(Product.Category));
            return Json(new { data = ProductsList });
        }
       
        public IActionResult Delete(int? id)
        {
            var ProdectToDelete = _UnitOfWork.Product.Get(temp=>temp.ID==id);
            if (ProdectToDelete == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            var OldImagePath = Path.Combine(_WebHostEnvironment.WebRootPath, ProdectToDelete.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(OldImagePath))
            {
                System.IO.File.Delete(OldImagePath);
            }
            _UnitOfWork.Product.Delete(ProdectToDelete);
            _UnitOfWork.Save();
            return Json(new { success=true, message="Delete Successful" });
        }

        #endregion
    }
}

