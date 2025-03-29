using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models.Entites;
using Models.identityEntites;
using Models.ViewModels;
using Stripe.Checkout;
using System.Security.Claims;
using Utilites;

namespace BookStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var UserID = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM = new ShoppingCartVM
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(temp => temp.ApplicationUserID == UserID, nameof(ShoppingCart.product)),
                OrderHeader=new()
                
            };

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }
            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int CartID)
        {
            var Cart = _unitOfWork.ShoppingCart.Get(temp => temp.ID == CartID);
            Cart.Count++;
            _unitOfWork.ShoppingCart.Update(Cart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int CartID)
        {
            var Cart = _unitOfWork.ShoppingCart.Get(temp => temp.ID == CartID);
            if (Cart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Delete(Cart);
            }
            else
            {
                Cart.Count--;
                _unitOfWork.ShoppingCart.Update(Cart);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int CartID)
        {
            var Cart = _unitOfWork.ShoppingCart.Get(temp => temp.ID == CartID);
            _unitOfWork.ShoppingCart.Delete(Cart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var UserID = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM = new ShoppingCartVM
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(temp => temp.ApplicationUserID == UserID, nameof(ShoppingCart.product)),
                OrderHeader = new()

            };
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.applicationUser.Get(x => x.Id == UserID);
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;


            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var UserID = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll(temp => temp.ApplicationUserID == UserID, nameof(ShoppingCart.product));
              

             ShoppingCartVM.OrderHeader.OrderDate=System.DateTime.Now;
             ShoppingCartVM.OrderHeader.ApplicationUserID=UserID;

            ApplicationUser applicationUser = _unitOfWork.applicationUser.Get(x => x.Id == UserID);
            

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }

            if (applicationUser.CompanyID.GetValueOrDefault() == 0)
            {
                //Customer is placing an Order
                ShoppingCartVM.OrderHeader.OrderStatus = StaticDetails.StatusPending;
                ShoppingCartVM.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusPending;
            }
            else
            {
                //company is placing an Order
                ShoppingCartVM.OrderHeader.OrderStatus = StaticDetails.StatusApproved;
                ShoppingCartVM.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusDelayedPayment;
            }
                _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
                _unitOfWork.Save();
            foreach(var cart in ShoppingCartVM.ListCart)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    ProductID = cart.ProductID,
                    OrderHeaderID = ShoppingCartVM.OrderHeader.ID,
                    Price=cart.Price,
                    Count=cart.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }
            if (applicationUser.CompanyID.GetValueOrDefault() == 0)
            {
                var domain = "https://localhost:7293/";
                //stripe logic
                var options = new Stripe.Checkout.SessionCreateOptions
                {

                    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.ID}",
                    CancelUrl = domain + "customer/cart/index",
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                    Mode = "payment",
                };

                foreach(var item in ShoppingCartVM.ListCart)
                {
                    var SessionLineItem = new SessionLineItemOptions()
                    {
                        PriceData = new SessionLineItemPriceDataOptions()
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.product.Title,

                            }
                        },
                        Quantity = item.Count

                    };
                    options.LineItems.Add(SessionLineItem);
                    
                }
                var service = new Stripe.Checkout.SessionService();
               Session session  = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStribePaymentID(ShoppingCartVM.OrderHeader.ID, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            return RedirectToAction("Confirmation");
        }
        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(temp => temp.ID == id);
            var Service = new SessionService();
            Session session = Service.Get(orderHeader.SesssionID);
            _unitOfWork.OrderHeader.UpdateStauts(id, StaticDetails.StatusApproved,StaticDetails.PaymentStatusApproved);
          List< ShoppingCart> shoppingCart = _unitOfWork.ShoppingCart.GetAll(temp => temp.ApplicationUserID == orderHeader.ApplicationUserID).ToList();
            _unitOfWork.ShoppingCart.DeleteRange(shoppingCart);
            _unitOfWork.Save();
            return View(id);
        }
        public double GetPriceBasedOnQuantity(ShoppingCart cart)
        {

            if (cart.Count <= 50)
            {
                return cart.product.Price;
            }
            else if (cart.Count <= 100)
            {
                return cart.product.Price50;
            }
            else
            {
                return cart.product.Price100;
            }
        }
    }
}
