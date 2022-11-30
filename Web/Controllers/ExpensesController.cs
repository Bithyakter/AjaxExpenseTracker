using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
   public class ExpensesController : Controller
   {
      public IActionResult Index()
      {
         return View();
      }

      [HttpGet]
      public IActionResult Create()
      {
         return View();
      }

      [HttpPost]
      public async Task<IActionResult> Create(string my)
      {
         return View(my);
      }

      public async Task<IActionResult> Details(int ID)
      {
         return View();
      }

      [HttpGet]
      public IActionResult Update()
      {
         return View();
      }

      [HttpPost]
      public async Task<IActionResult> Update(string my)
      {
         return View(my);
      }
   }
}