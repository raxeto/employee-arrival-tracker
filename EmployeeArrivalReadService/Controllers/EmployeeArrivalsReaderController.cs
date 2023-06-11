using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeArrivalData;
using EmployeeArrivalModels;
using System.Net;

namespace EmployeeArrivalReadService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeArrivalsReaderController : ControllerBase
    {
        private readonly EmployeeArrivalContext _dbContext;

        public EmployeeArrivalsReaderController(EmployeeArrivalContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index(string? sortOrder, string? nameSearch, DateTime? arrivalDateSearch)
        {
            try 
            {
                IQueryable<EmployeeArrival> result = _dbContext.EmployeeArrivals.Include(ea => ea.Employee).AsNoTracking();

                ApplySort(ref result, sortOrder);

                ApplySearch(ref result, nameSearch, arrivalDateSearch);

                var arrivals = await result.ToArrayAsync();

                return Ok(arrivals);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = $"Error during reading from database: {ex.Message}" });
            }
        }

        private void ApplySearch(ref IQueryable<EmployeeArrival> result, string? nameSearch, DateTime? arrivalDateSearch)
        {
            if (!string.IsNullOrEmpty(nameSearch))
            {
                var likeExpression = nameSearch + "%";

                //result = result.Where(e => EF.Functions.Like(e.Employee.Name, likeExpression) || EF.Functions.Like(e.Employee.SurName, likeExpression));
                result = result.Where(e => (e.Employee.Name + ' ' + e.Employee.SurName).Contains(nameSearch));
            }

            if (arrivalDateSearch != null)
            {
                DateTime dateMin = arrivalDateSearch.Value.Date;
                DateTime dateMax = arrivalDateSearch.Value.Date.AddDays(1).AddTicks(-1);

                result = result.Where(e => e.ArrivalDateTime >= dateMin && e.ArrivalDateTime <= dateMax);
            }
        }

        private void ApplySort(ref IQueryable<EmployeeArrival> result, string? sortOrder)
        {
            if (!string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = sortOrder.ToLower();

                switch (sortOrder)
                {
                    case "employeeid_asc":
                        result = result.OrderBy(e => e.EmployeeId);
                        break;
                    case "employeeid_desc":
                        result = result.OrderByDescending(e => e.EmployeeId);
                        break;
                    case "name_asc":
                        result = result.OrderBy(e => e.Employee.Name);
                        break;
                    case "name_desc":
                        result = result.OrderByDescending(e => e.Employee.Name);
                        break;
                    case "surname_asc":
                        result = result.OrderBy(e => e.Employee.SurName);
                        break;
                    case "surname_desc":
                        result = result.OrderByDescending(e => e.Employee.SurName);
                        break;
                    case "email_asc":
                        result = result.OrderBy(e => e.Employee.Email);
                        break;
                    case "email_desc":
                        result = result.OrderByDescending(e => e.Employee.Email);
                        break;
                    case "age_asc":
                        result = result.OrderBy(e => e.Employee.Age);
                        break;
                    case "age_desc":
                        result = result.OrderByDescending(e => e.Employee.Age);
                        break;
                    case "arrivaldatetime_asc":
                        result = result.OrderBy(e => e.ArrivalDateTime);
                        break;
                    case "arrivaldatetime_desc":
                        result = result.OrderByDescending(e => e.ArrivalDateTime);
                        break;
                }
            }
        }
    }
}
