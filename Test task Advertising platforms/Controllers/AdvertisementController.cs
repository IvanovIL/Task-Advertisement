using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Test_task_Advertising_platforms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdvertisementController : ControllerBase
    {

        private static List<Advertisement> AdvertisementLists = new List<Advertisement>();
        private readonly IConfiguration _configuration;


        private readonly ILogger<AdvertisementController> _logger;

        public AdvertisementController(ILogger<AdvertisementController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        ///  Загружает из файла json данные рекламы
        /// <summary>
        [HttpGet]
        [Route("Loading advertising platform data")]
        public async Task<IActionResult> Upload()
        {
            string path = _configuration.GetSection("PathInJson:Json").Value;

            string file = "";

            using (StreamReader streamReader = new StreamReader(path))
            {
                file = await streamReader.ReadToEndAsync();
            }

            AdvertisementLists = JsonConvert.DeserializeObject<List<Advertisement>>(file);

            return Ok("Äàííûå çàãðóæåíû");
        }

        /// <summary>
        /// Находит рекламные площадки по локациям
        /// </summary>
        /// <param name="NameDomen">Наименование локации</param>
        [HttpPost]
        [Route("Find ads by location")]
        public async Task<List<Advertisement>> FindAdvertisementAsync(string NameDomen)
        {
            List<Advertisement> advertisements = new List<Advertisement>();

            int id = 0;

            NameDomen = NameDomen.ToLower();

            if (NameDomen.StartsWith('/') == false)
            {
                NameDomen = "/" + NameDomen;
            }

            foreach (var advertisement in AdvertisementLists)
            {
                foreach (var item in advertisement.DomenName)
                {

                    if (item.Length <= NameDomen.Length)
                    {

                        if (NameDomen.EndsWith("/ru") | item.EndsWith("/ru"))
                        {
                            advertisements.Add(advertisement);
                        }
                        else if(item.Equals(NameDomen))
                        {
                            advertisements.Add(advertisement);
                        }
                        else if (NameDomen.Contains(item))
                        {
                            advertisements.Add(advertisement);
                        }


                    }
                }

            }

            return advertisements;

        }
    }
}
