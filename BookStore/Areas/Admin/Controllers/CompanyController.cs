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
    
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
            
        }
        public IActionResult Index()
        {
            List<Company> ComapnyList = _UnitOfWork.Company.GetAll().ToList();
            return View(ComapnyList);
        }
        public IActionResult Upsert(int? id)
        {
              

            if (id == null||id==0)
            {
                return View(new Company());
            }
            else
            {
             Company  company = _UnitOfWork.Company.Get(temp => temp.ID == id);
                return View(company);
            }
            
        }

        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (company.ID==0)
            {
                TempData["Success"] = "Company Added Succeccfully";
                _UnitOfWork.Company.Add(company);
            }
            else
            {
                TempData["Success"] = "Company Updated Succeccfully";
                _UnitOfWork.Company.Update(company);
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
            var ComapnyList=_UnitOfWork.Company.GetAll();
            return Json(new { data = ComapnyList });
        }
       
        public IActionResult Delete(int? id)
        {
            var CompanyToDelete = _UnitOfWork.Company.Get(temp=>temp.ID==id);
            if (CompanyToDelete == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
           
            _UnitOfWork.Company.Delete(CompanyToDelete);
            _UnitOfWork.Save();
            return Json(new { success=true, message="Delete Successful" });
        }

        #endregion
    }
}

