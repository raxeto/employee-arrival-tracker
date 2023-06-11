using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using EmployeeArrivalData;
using EmployeeArrivalModels;

namespace EmployeeArrivalWriteService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeArrivalsWritterController : ControllerBase
    {
        private readonly EmployeeArrivalContext _dbContext;
        public EmployeeArrivalsWritterController(EmployeeArrivalContext context)
        {
            _dbContext = context;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] IEnumerable<JsonEmployeeArrival> jsonArrivals)
        {
            if (ModelState.IsValid && jsonArrivals != null)
            {
                try
                {
                    var employeeArrivals = from a in jsonArrivals
                                           select new EmployeeArrival()
                                           {
                                               EmployeeId = a.EmployeeId,
                                               ArrivalDateTime = a.When
                                           };

                    _dbContext.EmployeeArrivals.AddRange(employeeArrivals);
                    
                    await _dbContext.SaveChangesAsync();

                    return Ok(new { message = "Data collection submitted successfully." });
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
        }
    }
}
