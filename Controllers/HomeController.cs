using IntexFagElGamous.Models.ViewModels;
using IntexFagElGamous.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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
            IQueryable<Textile> filteredTextiles = IntexContext.Textiles;
            IQueryable<Color> filteredColors = IntexContext.Colors;

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

            if (filter.HeadWest)
            {
                if (filter.HeadEast)
                {
                    filteredBurials = filteredBurials.Where(x => x.Headdirection == "E" || x.Headdirection == "W");
                }
                else
                {
                    filteredBurials = filteredBurials.Where(x => x.Headdirection == "W");
                }
            }
            else if (filter.HeadEast)
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

            var age = new List<string>();

            if (filter.Adult)
            {
                age.Add("A");
            }
            if (filter.Child)
            {
                age.Add("C");
            }
            if (filter.Infant)
            {
                age.Add("I");
            }
            if (filter.Newborn)
            {
                age.Add("N");
            }
            if (age.Count > 0)
            {
                filteredBurials = filteredBurials.Where(x => age.Contains(x.Ageatdeath));
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

            var wrappingList = new List<string>();

            if (filter.Full)
            {
                wrappingList.Add("W");
            }

            if (filter.Partial)
            {
                wrappingList.Add("H");
            }

            if (filter.Bones)
            {
                wrappingList.Add("B");
            }
            if (wrappingList.Count > 0)
            {
                filteredBurials = filteredBurials.Where(x => wrappingList.Contains(x.Wrapping));
            }

            if (filter.TextileColor == "Green")
            {
                var colors = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join Color_Textile ct on t.Id = ct.main$textileid " +
                    $"join Color c on ct.main$colorid = c.Id " +
                    $"where c.value like '%Green%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => colors.Contains(x));
            }
            if (filter.TextileColor == "Black")
            {
                var colors = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join Color_Textile ct on t.Id = ct.main$textileid " +
                    $"join Color c on ct.main$colorid = c.Id " +
                    $"where c.value like '%Black%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => colors.Contains(x));
            }
            if (filter.TextileColor == "Blue")
            {
                var colors = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join Color_Textile ct on t.Id = ct.main$textileid " +
                    $"join Color c on ct.main$colorid = c.Id " +
                    $"where c.value like '%Blue%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => colors.Contains(x));
            }
            if (filter.TextileColor == "Brown")
            {
                var colors = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join Color_Textile ct on t.Id = ct.main$textileid " +
                    $"join Color c on ct.main$colorid = c.Id " +
                    $"where c.value like '%Brown%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => colors.Contains(x));
            }
            if (filter.TextileColor == "Yellow")
            {
                var colors = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join Color_Textile ct on t.Id = ct.main$textileid " +
                    $"join Color c on ct.main$colorid = c.Id " +
                    $"where c.value like '%Yellow%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => colors.Contains(x));
            }
            if (filter.TextileColor == "Orange")
            {
                var colors = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join Color_Textile ct on t.Id = ct.main$textileid " +
                    $"join Color c on ct.main$colorid = c.Id " +
                    $"where c.value like '%Oranged%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => colors.Contains(x));
            }
            if (filter.TextileColor == "Purple")
            {
                var colors = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join Color_Textile ct on t.Id = ct.main$textileid " +
                    $"join Color c on ct.main$colorid = c.Id " +
                    $"where c.value like '%Purple%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => colors.Contains(x));
            }
            if (filter.TextileColor == "Red")
            {
                var colors = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join Color_Textile ct on t.Id = ct.main$textileid " +
                    $"join Color c on ct.main$colorid = c.Id " +
                    $"where c.value like '%Red%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => colors.Contains(x));
            }
            if (filter.TextileColor == "Undyed")
            {
                var colors = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join Color_Textile ct on t.Id = ct.main$textileid " +
                    $"join Color c on ct.main$colorid = c.Id " +
                    $"where c.value like '%Undyed%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => colors.Contains(x));
            }
            if (filter.TextileColor == "White")
            {
                var colors = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join Color_Textile ct on t.Id = ct.main$textileid " +
                    $"join Color c on ct.main$colorid = c.Id " +
                    $"where c.value like '%White%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => colors.Contains(x));
            }

            if (filter.East)
            {
                if (filter.West)
                {
                    filteredBurials = filteredBurials.Where(x => x.Eastwest == "E" || x.Eastwest == "W");
                }
                else
                {
                    filteredBurials = filteredBurials.Where(x => x.Eastwest == "E");
                }

            }
            else if (filter.West)
            {
                filteredBurials = filteredBurials.Where(x => x.Eastwest == "W");
            }

            if (filter.North)
            {
                if (filter.South)
                {
                    filteredBurials = filteredBurials.Where(x => x.Northsouth == "N" || x.Northsouth == "S");
                }
                else
                {
                    filteredBurials = filteredBurials.Where(x => x.Northsouth == "N");
                }

            }
            else if (filter.South)
            {
                filteredBurials = filteredBurials.Where(x => x.Northsouth == "S");
            }

            if (filter.SquareEastWest != null)
            {
                filteredBurials = filteredBurials.Where(x => Convert.ToDecimal(x.Squareeastwest) == filter.SquareEastWest);
            }

            if (filter.SquareNorthSouth != null)
            {
                filteredBurials = filteredBurials.Where(x => Convert.ToDecimal(x.Squarenorthsouth) == filter.SquareNorthSouth);
            }

            var area = new List<string>();

            if (filter.SouthWestArea)
            {
                area.Add("SW");
            }

            if (filter.SouthEastArea)
            {
                area.Add("SE");
            }

            if (filter.NorthEastArea)
            {
                area.Add("NE");
            }
            if (filter.NorthWestArea)
            {
                area.Add("NW");
            }
            if (area.Count > 0)
            {
                filteredBurials = filteredBurials.Where(x => area.Contains(x.Area));
            }

            if (filter.BurialNumber != null)
            {
                filteredBurials = filteredBurials.Where(x => Convert.ToDecimal(x.Burialnumber) == filter.BurialNumber);
            }

            if (filter.TextileFunction == "Blanket")
            {
                var textiles = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join textilefunction_textile tft on t.Id = tft.main$textileid " +
                    $"join textilefunction tf on tft.main$textilefunctionid = tf.id " +
                    $"where tf.value like 'Blanket%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => textiles.Contains(x));
            }
            if (filter.TextileFunction == "Cordage")
            {
                var textiles = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join textilefunction_textile tft on t.Id = tft.main$textileid " +
                    $"join textilefunction tf on tft.main$textilefunctionid = tf.id " +
                    $"where tf.value like 'Cordage%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => textiles.Contains(x));
            }
            if (filter.TextileFunction == "Face")
            {
                var textiles = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join textilefunction_textile tft on t.Id = tft.main$textileid " +
                    $"join textilefunction tf on tft.main$textilefunctionid = tf.id " +
                    $"where tf.value like 'Face%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => textiles.Contains(x));
            }
            if (filter.TextileFunction == "Fragmentary")
            {
                var textiles = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join textilefunction_textile tft on t.Id = tft.main$textileid " +
                    $"join textilefunction tf on tft.main$textilefunctionid = tf.id " +
                    $"where tf.value like 'Fragmentary%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => textiles.Contains(x));
            }
            if (filter.TextileFunction == "Head")
            {
                var textiles = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join textilefunction_textile tft on t.Id = tft.main$textileid " +
                    $"join textilefunction tf on tft.main$textilefunctionid = tf.id " +
                    $"where tf.value like 'Head%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => textiles.Contains(x));
            }
            if (filter.TextileFunction == "Other")
            {
                var textiles = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join textilefunction_textile tft on t.Id = tft.main$textileid " +
                    $"join textilefunction tf on tft.main$textilefunctionid = tf.id " +
                    $"where tf.value like 'Other%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => textiles.Contains(x));
            }
            if (filter.TextileFunction == "Ribbon")
            {
                var textiles = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join textilefunction_textile tft on t.Id = tft.main$textileid " +
                    $"join textilefunction tf on tft.main$textilefunctionid = tf.id " +
                    $"where tf.value like 'Ribbon%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => textiles.Contains(x));
            }
            if (filter.TextileFunction == "Tunic")
            {
                var textiles = IntexContext.Burialmains
                    .FromSqlRaw("select b.* from Burialmain b " +
                    $"join Burialmain_Textile bt on b.Id = bt.main$burialmainid " +
                    $"join Textile t on bt.main$textileid = t.Id " +
                    $"join textilefunction_textile tft on t.Id = tft.main$textileid " +
                    $"join textilefunction tf on tft.main$textilefunctionid = tf.id " +
                    $"where tf.value like 'Tunic%'")
                    .Distinct()
                    .ToList();
                filteredBurials = filteredBurials.Where(x => textiles.Contains(x));
            }

            if (filter.FaceBundle == "Yes")
            {
                filteredBurials = filteredBurials.Where(x => x.Facebundles == "Y");
            }



            List<Burialmain> resultsBurials = filteredBurials.ToList();

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

        [Authorize(Roles = "Admin, Researcher")]
        [HttpGet]
        public IActionResult CRUDadd()
        {

            var maxId = IntexContext.Burialmains.Max(x => x.Id);
            var burial = new Burialmain { Id = maxId + 1 };
            return View(burial);
        }

        [Authorize(Roles = "Admin, Researcher")]
        [HttpPost]
        public IActionResult CRUDadd(Burialmain burial)
        {

            IntexContext.Burialmains.Add(burial);
            IntexContext.SaveChanges();
            return View("CRUDconfirm", burial);
        }

        [Authorize(Roles = "Admin, Researcher")]
        [HttpGet]
        public IActionResult CRUDdelete(long id)
        {
            var burial = IntexContext.Burialmains.Single(x => x.Id == id);
            return View(burial);
        }

        [Authorize(Roles = "Admin, Researcher")]
        [HttpPost]
        public IActionResult CRUDdelete(Burialmain burial)
        {
            IntexContext.Burialmains.Remove(burial);
            IntexContext.SaveChanges();
            return RedirectToAction("Burials");
        }

        [Authorize(Roles = "Admin, Researcher")]
        [HttpGet]
        public IActionResult CRUDedit(long id)
        {
            var burial = IntexContext.Burialmains.Single(x => x.Id == id);
            return View("CRUDadd", burial);
        }
        [Authorize(Roles = "Admin, Researcher")]
        [HttpPost]
        public IActionResult CRUDedit(Burialmain burial)
        {
            IntexContext.Update(burial);
            IntexContext.SaveChanges();
            return RedirectToAction("Summary", burial);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { ErrorMessage = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public ActionResult Supervised(ErrorViewModel error)
        {
            return View(error);
        }

        [HttpPost]
        public async Task<IActionResult> Supervised(float depth, string sex, string adultchild, string goods, string wrapping)
        {
            // Set the API endpoint URL
            string apiUrl = "http://3.140.174.250/predict";

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

            Task<HttpResponseMessage> task = _httpClient.PostAsync(apiUrl, content);
            Task completedTask = await Task.WhenAny(task, Task.Delay(TimeSpan.FromSeconds(5)));

            if (completedTask == task)
            {

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
                    ErrorViewModel errorMessage = new ErrorViewModel { ErrorMessage= $"Prediction API error ({response.StatusCode}): {responseContent}" } ;
                    return RedirectToAction("Supervised", errorMessage);
                }
            }
            else
            {
                // The API call took more than 5 seconds
                ErrorViewModel errorMessage = new ErrorViewModel { ErrorMessage = "An error occurred while making a mummy head direction prediction." };
                return RedirectToAction("Supervised", errorMessage);
                //return RedirectToAction("Index");

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