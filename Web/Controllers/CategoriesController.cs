using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
   public class CategoriesController : Controller
   {
      public IActionResult Index()
      {
         return View();
      }
   }
}