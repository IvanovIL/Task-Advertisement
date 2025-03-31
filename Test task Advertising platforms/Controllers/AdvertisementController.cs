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
        ///  –ó–∞–≥—Ä—É–∂–∞–µ—Ç –∏–∑ —Ñ–∞–π–ª–∞ json –¥–∞–Ω–Ω—ã–µ —Ä–µ–∫–ª–∞–º—ã
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

            return Ok("√Ñ√†√≠√≠√ª√• √ß√†√£√∞√≥√¶√•√≠√ª");
        }

        /// <summary>
        /// –ù–∞—Ö–æ–¥–∏—Ç —Ä–µ–∫–ª–∞–º–Ω—ã–µ –ø–ª–æ—â–∞–¥–∫–∏ –ø–æ –ª–æ–∫–∞—Ü–∏—è–º
        /// </summary>
        /// <param name="NameDomen">–ù–∞–∏–º–µ–Ω–æ–≤–∞–Ω–∏–µ –ª–æ–∫–∞—Ü–∏–∏</param>
        [HttpPost]
        [Route("Find ads by location")]
        public async Task<List<Advertisement>> FindAdvertisementAsync(string NameDomen)
        {
            List<Advertisement> advertisements = new List<Advertisement>();

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

                        if (NameDomen.StartsWith("ru") & item.EndsWith("ru"))
                        {
                            advertisements.Add(advertisement);
                        }
                        else if (item.Equals(NameDomen))
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
            if (advertisements.Count == 0)
            {
                advertisements.Add(new Advertisement
                {
                    Name = "ÕÂ Ì‡È‰ÂÌ‡ ÂÍÎ‡ÏÌ‡ˇ ÔÎÓ˘‡‰Í‡",
                    DomenName = ["œÓ‚Â¸ÚÂ Ô‡‚ËÎ¸ÌÓÒÚ¸ Ì‡ÔËÒ‡ÌËˇ ÎÓÍ‡ˆËË, Ú‡Í ÊÂ Ì‡ÔËÒ‡ÌËÂ ÒÚÓ„Ó Ì‡ ‡Ì„ÎËÈÒÍÓÏ"],
                });
            }
            return advertisements;

        }
    }
}
