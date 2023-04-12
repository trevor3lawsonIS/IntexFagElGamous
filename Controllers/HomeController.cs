﻿using IntexFagElGamous.Models.ViewModels;
using IntexFagElGamous.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace IntexFagElGamous.Controllers
{
    public class HomeController : Controller
    {
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

            ViewBag.Changes = new FilterViewModel();

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

            if (filter.Male)
            {
                if (filter.Female)
                {
                    filteredBurials = filteredBurials.Where(x => x.Sex == "F" || x.Sex == "M");
                }
                else
                {
                    filteredBurials = filteredBurials.Where(x => x.Sex == "M");
                }
                
            }
            else if (filter.Female)
            {
                filteredBurials = filteredBurials.Where(x => x.Sex == "F");
            }

            if (filter.West)
            {
                if (filter.East)
                {
                    filteredBurials = filteredBurials.Where(x => x.Headdirection == "E" || x.Headdirection == "W");
                }
                else
                {
                    filteredBurials = filteredBurials.Where(x => x.Headdirection == "W");
                }
            }
            else if (filter.East)
            {
                filteredBurials = filteredBurials.Where(x => x.Headdirection == "E");
            }

            if (filter.MinDepth != null)
            {
                filteredBurials = filteredBurials.Where(x => Convert.ToDecimal(x.Depth) >= filter.MinDepth);
            }

            if (filter.MaxDepth != null)
            {
                filteredBurials = filteredBurials.Where(x => Convert.ToDecimal(x.Depth) <= filter.MaxDepth);
            }

            if (filter.Adult)
            {
                if (filter.Child)
                {
                    filteredBurials = filteredBurials.Where(x => x.Adultsubadult == "C" || x.Adultsubadult == "A");
                }
                else
                {
                    filteredBurials = filteredBurials.Where(x => x.Adultsubadult == "A");
                }
            }
            else if (filter.Child)
            {
                filteredBurials = filteredBurials.Where(x => x.Adultsubadult == "C");
            }

            var hairColors = new List<string>();

            if (filter.Brown)
            {
                hairColors.Add("B");
            }

            if (filter.Black)
            {
                hairColors.Add("K");
            }

            if (filter.BrownRed)
            {
                hairColors.Add("A");
            }

            if (filter.Red)
            {
                hairColors.Add("R");
            }

            if (filter.Blond)
            {
                hairColors.Add("D");
            }

            if (filter.Unknown)
            {
                hairColors.Add("U");
            }

            if (hairColors.Count > 0)
            {
                filteredBurials = filteredBurials.Where(x => hairColors.Contains(x.Haircolor));
            }

            var wrapping = new List<string>();

            if (filter.Full)
            {
                wrapping.Add("W");
            }

            if (filter.Partial)
            {
                wrapping.Add("H");
            }

            if (filter.Bones)
            {
                wrapping.Add("B");
            }
            if (wrapping.Count > 0)
            {
                filteredBurials = filteredBurials.Where(x => wrapping.Contains(x.Wrapping));
            }

            List<Burialmain> results = filteredBurials.ToList();

            ViewBag.Changes = filter;

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

        public IActionResult Summary(long id)
        {
            var burial = IntexContext.Burialmains.Single(x=>x.Id == id);
            return View(burial);
        }

        public IActionResult Supervised()
        {
            return View();
        }
      

        [HttpGet]
        public IActionResult Unsupervised()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Unsupervised(ChartModel x)
        {
            return View(x);
        }

        [HttpGet]
        public IActionResult CRUDadd()
        {
            return View("CRUDadd", new Burialmain());
        }
        [HttpPost]
        public IActionResult CRUDadd(Burialmain burial)
        {
            IntexContext.Add(burial);
            IntexContext.SaveChanges();
            return View("CRUDconfirm", burial);
        }

        [HttpGet]
        public IActionResult CRUDdelete(int id)
        {
            var burial = IntexContext.Burialmains.Single(x => x.Id == id);
            return View(burial);
        }
        [HttpPost]
        public IActionResult CRUDdelete(Burialmain burial)
        {
            IntexContext.Burialmains.Remove(burial);
            IntexContext.SaveChanges();
            return RedirectToAction("Burials");
        }

        [HttpGet]
        public IActionResult CRUDedit(int id)
        {
            var burial = IntexContext.Burialmains.Single(x => x.Id == id);
            return View("CRUDadd", burial);
        }
        [HttpPost]
        public IActionResult CRUDedit(Burialmain burial)
        {
            IntexContext.Update(burial);
            IntexContext.SaveChanges();
            return RedirectToAction("Burials");
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