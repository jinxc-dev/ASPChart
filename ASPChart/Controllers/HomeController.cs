using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPChart.Models;

namespace ASPChart.Controllers
{
    public class HomeController : Controller
    {
        private IMemberCURDModel _member;

        public HomeController(IMemberCURDModel member)
        {
            _member = member;        
        }

        public IActionResult Index()
        {
            //var list = _member.GetAll();
            return View();
        }

        public IActionResult Test()
        {
            string value = HttpContext.Request.Query["LT"];

            //var list = _member.GetAll(value);

            //var model = _member.GetAll(value);
            ViewData["param"] = value;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
