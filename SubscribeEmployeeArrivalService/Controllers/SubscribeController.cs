using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using SubscribeEmployeeArrivalService.Data;
using SubscribeEmployeeArrivalService.Models;
using Microsoft.EntityFrameworkCore;

namespace SubscribeEmployeeArrivalService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly SubscriptionTokensContext _dbContext;

        public SubscribeController(IConfiguration configuration, IHttpClientFactory httpClientFactory, SubscriptionTokensContext dbContext)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
            _dbContext = dbContext;
        }

        [HttpGet("validate-token")]
        public async Task<IActionResult> ValidateToken(string? token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            try
            {
                bool tokenValid = await _dbContext.Tokens.Where(t => t.TokenValue == token && t.Expires >= DateTime.Now).AnyAsync();
                return Ok(tokenValid);
            }
            catch (Exception ex) 
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = $"Error during subscription: {ex.Message}" });
            }
        }

        [HttpPost("new")]
        public async Task<IActionResult> NewSubscription([FromBody] SubscribeData data)
        {
            string date = data.Date;
            string callback = data.Callback;

            string requestUrl = _configuration.GetValue<string>("ExternalService:FourthSubscribeUrl") +
                 $"?date={date}&callback={callback}";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Accept-Client", "Fourth-Monitor");

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                else if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(jsonResponse);

                    if (tokenResponse != null)
                    {
                        _dbContext.Tokens.Add(new Token()
                        {
                            TokenValue = tokenResponse.Token,
                            Expires = tokenResponse.Expires
                        });

                        await _dbContext.SaveChangesAsync();

                        return Ok(); 
                    }
                    else
                    {
                        return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Invalid token." });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = $"Error during subscription: {ex.Message}" });
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
