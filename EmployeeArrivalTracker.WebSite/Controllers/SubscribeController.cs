using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace EmployeeArrivalTracker.WebSite.Controllers
{
    public class SubscribeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public SubscribeController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(DateTime date)
        {
            string callback = _configuration.GetValue<string>("InternalService:TrackArrivalService") ?? string.Empty;

            string requestUrl = _configuration.GetValue<string>("InternalService:SubscribeServiceNew") ?? string.Empty ;

            var values = new Dictionary<string, string>();
            values.Add("date", date.ToString("yyyy-MM-dd"));
            values.Add("callback", Uri.EscapeDataString(callback));

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUrl, values);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Success", "Subscribe");
                }
                else
                {
                    return RedirectToAction("Error", "Subscribe", new { message = "Error saving tocken" });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Subscribe", new { message = $"Internal error: {ex.Message}" });
            }
        }


        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Error(string message)
        {
            ViewData["ErrorMessage"] = !string.IsNullOrEmpty(message) ? message : "An unexpected error occurred.";
            return View();
        }
    }
}
