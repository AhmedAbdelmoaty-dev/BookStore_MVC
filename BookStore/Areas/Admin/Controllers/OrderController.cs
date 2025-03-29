using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Models.Entites;
using Models.identityEntites;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            
            return View();
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<OrderHeader> OrderList = _UnitOfWork.OrderHeader.GetAll(includeProperties: nameof(ApplicationUser)).ToList();
            return Json(new { data = OrderList });
        }

       

        #endregion
    }
}
