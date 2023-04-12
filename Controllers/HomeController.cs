using IntexFagElGamous.Models.ViewModels;
using IntexFagElGamous.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;

namespace IntexFagElGamous.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        private intexmummyContext IntexContext { get; set; }

        public HomeController(intexmummyContext intexContext)
        {
            IntexContext = intexContext;
            _httpClient = new HttpClient();
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

            //if (filter.MinDepth != null)
            //{
            //    filteredBurials = filteredBurials.Where(x => int.Parse(x.Depth) >= int.Parse(filter.MinDepth));
            //}

            //if (filter.MaxDepth != null)
            //{
            //    filteredBurials = filteredBurials.Where(x => int.Parse(x.Depth) <= int.Parse(filter.MaxDepth));
            //}

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

            // ADD THIS FUNCTIONALITY LATER! FIND A WAY THAT DOES NOT INCLUDE 100000 NESTED IF STATEMENTS
            if (filter.Brown)
            {
                filteredBurials = filteredBurials.Where(x => x.Haircolor == "B");
            }

            if (filter.Black)
            {
                filteredBurials = filteredBurials.Where(x => x.Haircolor == "K");
            }

            if (filter.BrownRed)
            {
                filteredBurials = filteredBurials.Where(x => x.Haircolor == "A");
            }

            if (filter.Red)
            {
                filteredBurials = filteredBurials.Where(x => x.Haircolor == "R");
            }

            if (filter.Blond)
            {
                filteredBurials = filteredBurials.Where(x => x.Haircolor == "D");
            }

            if (filter.Unknown)
            {
                filteredBurials = filteredBurials.Where(x => x.Haircolor == "U");
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
            var maxId = IntexContext.Burialmains.Max(x => x.Id);
            var burial = new Burialmain { Id = maxId + 1 };
            return View(burial);
        }
        
        [HttpPost]
        public IActionResult CRUDadd(Burialmain burial)
        {

            IntexContext.Burialmains.Add(burial);
            IntexContext.SaveChanges();
            return View("CRUDconfirm", burial);
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


        [HttpGet]
        public ActionResult Supervised()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Supervised(float depth, string sex, string adultchild, string goods, string wrapping)
        {
            // Set the API endpoint URL
            string apiUrl = "http://52.70.13.136/predict";

            int adultsubadult_C;
            int goods_Other;
            int wrapping_H;
            int wrapping_W;

            // Create an input object from the user-provided data
            if (adultchild == "Adult")
            {
                adultsubadult_C = 0;
            }
            else
            {
                adultsubadult_C = 1;

            }

            if (goods == "Yes")
            {
                goods_Other = 1;
            }
            else
            {
                goods_Other = 0;

            }

            if (wrapping == "Whole")
            {
                wrapping_H = 0;
                wrapping_W = 1;
            }
            else if (wrapping == "Half")
            {
                wrapping_H = 1;
                wrapping_W = 0;
            }
            else
            {
                wrapping_H = 0;
                wrapping_W = 0;
            }

            var inputData = new { depth = depth, adultsubadult_C = adultsubadult_C, goods_Other = goods_Other, wrapping_H = wrapping_H, wrapping_W = wrapping_W };

            // Serialize the input object to JSON
            string inputDataJson = JsonConvert.SerializeObject(inputData);

            // Set the content type and the input data in the request body
            StringContent content = new StringContent(inputDataJson, System.Text.Encoding.UTF8, "application/json");

            // Send the POST request to the API endpoint and wait for the response
            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);
            string responseContent = await response.Content.ReadAsStringAsync();

            // Check if the request was successful (status code 200)
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the JSON response into a result object
                ModelResult ResultFromApi = JsonConvert.DeserializeObject<ModelResult>(responseContent);

                // Pass the prediction result to the view and render it

                //TempData["Result"] = Result;

                return RedirectToAction("MyResult", ResultFromApi);

            }
            else
            {
                // Pass the error message to the view and render it
                string errorMessage = $"Prediction API error ({response.StatusCode}): {responseContent}";
                return View("PredictionError", errorMessage);
            }
        }

        [HttpGet]
        public ActionResult MyResult(int ResultFromApi)
        {
            string ResultString = "";

            if (ResultFromApi == 1)
            {
                ResultString = "West";
            }
            else
            {
                ResultString = "East";
            }

            PredictionResult result = new PredictionResult { Result = ResultString };
            return View(result);
        }

        //Define a class to deserialize the JSON response into
        public class ModelResult
        {
            public float Prediction { get; set; }
        }

    }
}