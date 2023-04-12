using IntexFagElGamous.Models.ViewModels;
using IntexFagElGamous.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IntexFagElGamous.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private intexmummyContext IntexContext { get; set; }

        public HomeController(intexmummyContext intexContext)
        {
            IntexContext = intexContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Burials(int pageNum = 1)
        {
            int pageSize = 20;

            var x = new BurialViewModel
            {
                Burialmains = IntexContext.Burialmains
                .OrderBy(x => x.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                PageInfo = new PageInfo
                {
                    TotalNumBurials = IntexContext.Burialmains.Count(),
                    BurialsPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };

            return View(x);
        }

        [HttpPost]
        public IActionResult Burials([FromForm] FilterViewModel filter, int pageNum = 1)
        {

            IQueryable<Burialmain> filteredBurials = IntexContext.Burialmains;

            if (filter.Male == "M")
            {
                filteredBurials = filteredBurials.Where(x => x.Sex == "M");
            }

            if (filter.Female == "F")
            {
                filteredBurials = filteredBurials.Where(x => x.Sex == "F");
            }

            if (filter.West == "W")
            {
                filteredBurials = filteredBurials.Where(x => x.Headdirection == "W");
            }

            if (filter.East == "E")
            {
                filteredBurials = filteredBurials.Where(x => x.Headdirection == "E");
            }

            //if (filter.MinDepth != null)
            //{
            //    filteredBurials = filteredBurials.Where(x => int.Parse(x.Depth) >= int.Parse(filter.MinDepth));
            //}

            //if (filter.MaxDepth != null)
            //{
            //    filteredBurials = filteredBurials.Where(x => int.Parse(x.Depth) <= int.Parse(filter.MaxDepth));
            //}

            if (filter.Adult == "A")
            {
                filteredBurials = filteredBurials.Where(x => x.Adultsubadult == "A");
            }

            if (filter.Child == "C")
            {
                filteredBurials = filteredBurials.Where(x => x.Adultsubadult == "C");
            }

            if (filter.Brown == "B")
            {
                filteredBurials = filteredBurials.Where(x => x.Haircolor == "B");
            }

            if (filter.Black == "K")
            {
                filteredBurials = filteredBurials.Where(x => x.Haircolor == "K");
            }

            if (filter.BrownRed == "A")
            {
                filteredBurials = filteredBurials.Where(x => x.Haircolor == "A");
            }

            if (filter.Red == "R")
            {
                filteredBurials = filteredBurials.Where(x => x.Haircolor == "R");
            }

            if (filter.Blond == "D")
            {
                filteredBurials = filteredBurials.Where(x => x.Haircolor == "D");
            }

            if (filter.Unknown == "U")
            {
                filteredBurials = filteredBurials.Where(x => x.Haircolor == "U");
            }

            List<Burialmain> results = filteredBurials.ToList();

            var pageSize = 20;
            var x = new BurialViewModel
            {
                Burialmains = filteredBurials
                .OrderBy(x => x.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                PageInfo = new PageInfo
                {
                    TotalNumBurials = filteredBurials.Count(),
                    BurialsPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };

            return View(x);
        }



        public IActionResult Supervised()
        {
            return View();
        }

        public IActionResult Unsupervised()
        {
            return View();
        }

        public IActionResult CRUD()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}