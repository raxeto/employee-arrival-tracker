using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using EmployeeArrivalTracker.WebSite.Helpers;
using EmployeeArrivalModels;


namespace EmployeeArrivalTracker.WebSite.Controllers
{
    public class EmployeeArrivalsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public EmployeeArrivalsController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<IActionResult> Index(string sortOrder, string nameSearch, DateTime? arrivalDateSearch)
        {
            string arrivalDateStr = arrivalDateSearch.HasValue ? arrivalDateSearch.Value.ToString("yyyy-MM-dd") : string.Empty;

            string requestUrl = _configuration.GetValue<string>("InternalService:EmployeeArrivalRead") +
               $"?sortOrder={sortOrder}&nameSearch={nameSearch}&arrivalDateSearch={arrivalDateStr}";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var arrivals = JsonSerializer.Deserialize<EmployeeArrival[]>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (arrivals != null)
                    {
                        SaveSortViewData(sortOrder);

                        SaveSearchViewData(nameSearch, arrivalDateSearch);

                        return View(arrivals);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = $"Error during getting information from service: {ex.Message}" });
            }

            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Error while retrieving data." });
        }

        private void SaveSearchViewData(string nameSearch, DateTime? arrivalDateSearch)
        {
            ViewData["NameSearchPar"] = nameSearch;
            ViewData["ArrivalDateSearchPar"] = arrivalDateSearch != null ? arrivalDateSearch.Value.ToString("yyyy-MM-dd") : string.Empty;
        }

        private void SaveSortViewData(string sortOrder)
        {
            string[] sortingFields = new string[]
            {
                "EmployeeId",
                "Name",
                "SurName",
                "Email",
                "Age",
                "ArrivalDateTime"
            };

            foreach (var field in sortingFields)
            {
                string lowerField = field.ToLower();

                string parValue;

                if (string.IsNullOrEmpty(sortOrder) || sortOrder == lowerField + "_desc" || !sortOrder.StartsWith(lowerField))
                {
                    parValue = lowerField + "_asc";
                }
                else
                {
                    parValue = lowerField + "_desc";
                }

                ViewData[field + "SortPar"] = parValue;
            }
        }
    }
}
