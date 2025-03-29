using DataAccess.Data;
using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entites;
using Utilites;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles =StaticDetails.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            List<Category> categories = _UnitOfWork.Category.GetAll().ToList();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Category Name and Display Order Cannot be exactly same");
            }
            if (ModelState.IsValid)
            {
                TempData["Success"] = "Category Created Succeccfully";
                _UnitOfWork.Category.Add(obj);
                _UnitOfWork.Save();
            }
            return RedirectToAction("Index");

        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? CategoryFromDb = _UnitOfWork.Category.Get(temp => temp.ID == id);
            if (CategoryFromDb == null)
            {
                return NotFound();
            }
            return View(CategoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Update(category);
                _UnitOfWork.Save();
                TempData["Success"] = "Category Edited Succeccfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? CategoryFromDb = _UnitOfWork.Category.Get(temp => temp.ID == id);
            if (CategoryFromDb == null)
            {
                return NotFound();
            }
            return View(CategoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _UnitOfWork.Category.Get(temp => temp.ID == id);
            if (obj == null)
            {
                return NotFound();
            }

            _UnitOfWork.Category.Delete(obj);
            _UnitOfWork.Save();
            TempData["Success"] = "Category Deleted Succeccfully";
            return RedirectToAction("index");
        }
    }
}
