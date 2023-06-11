using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using EmployeeArrivalModels;
using EmployeeArrivalData;

namespace EmployeeArrivalTracker.WebSite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackArrivalController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public TrackArrivalController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }


        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] IEnumerable<JsonEmployeeArrival> jsonArrivals)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var requestToken = Request.Headers["X-Fourth-Token"].ToString();

                    string tockenRequestUrl = _configuration.GetValue<string>("InternalService:SubscribeServiceValidateToken") +
                       $"?token={requestToken}";

                    var tockenRequest = new HttpRequestMessage(HttpMethod.Get, tockenRequestUrl);

                    HttpResponseMessage tokenResponse;
                    tokenResponse = await _httpClient.SendAsync(tockenRequest);

                    if (tokenResponse.IsSuccessStatusCode)
                    {
                        var tockenJson = await tokenResponse.Content.ReadAsStringAsync();
                        var tokenRes = JsonSerializer.Deserialize<bool>(tockenJson);

                        if (tokenRes)
                        {
                            string writeArrivalsUrl = _configuration.GetValue<string>("InternalService:EmployeeArrivalWrite") ?? string.Empty;

                            HttpResponseMessage writeArrivalsResponse = await _httpClient.PostAsJsonAsync(writeArrivalsUrl, jsonArrivals);

                            if (writeArrivalsResponse.IsSuccessStatusCode)
                            {
                                return Ok(new { message = "Data collection submitted successfully." });
                            }
                            else
                            {
                                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Error while saving arrivals." });
                            }
                        }
                        else
                        {
                            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Invalid token." });
                        }
                    }
                }
                catch (Exception)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An error occurred while saving the data." });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
