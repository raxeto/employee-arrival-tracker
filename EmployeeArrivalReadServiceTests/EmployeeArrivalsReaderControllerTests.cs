using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using EmployeeArrivalReadService.Controllers;
using EmployeeArrivalData;
using EmployeeArrivalTestDataSeeder;
using EmployeeArrivalModels;

namespace EmployeeArrivalReadServiceTests
{
    public class EmployeeArrivalsReaderControllerTests : IDisposable
    {
        private readonly EmployeeArrivalContext _context;

        public EmployeeArrivalsReaderControllerTests()
        {
            var options = new DbContextOptionsBuilder<EmployeeArrivalContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new EmployeeArrivalContext(options);
            _context.Database.EnsureCreated();

            TestDataSeeder.SeedData(_context);
        }

        [Fact]
        public async Task Index_WithoutParameters_ReturnsOkObjectResult()
        {
            var controller = new EmployeeArrivalsReaderController(_context);

            IActionResult result = await controller.Index(null, null, null);

            var viewResult = Assert.IsType<OkObjectResult>(result);

            var model = Assert.IsAssignableFrom<EmployeeArrival[]>(viewResult.Value);
            var employeeArrivals = await _context.EmployeeArrivals.ToArrayAsync();

            Assert.Equal(employeeArrivals.Length, model.Length);

            Assert.Equal(employeeArrivals, model);
        }

        // Test with sortOrder parameter
        [Fact]
        public async Task Index_WithSortOrder_ReturnsOkObjectResult()
        {
            var controller = new EmployeeArrivalsReaderController(_context);

            IActionResult result = await controller.Index("name_desc", null, null);

            var viewResult = Assert.IsType<OkObjectResult>(result);

            var model = Assert.IsAssignableFrom<EmployeeArrival[]>(viewResult.Value);
            var employeeArrivals = await _context.EmployeeArrivals.OrderByDescending(a => a.Employee.Name).ToArrayAsync();

            Assert.Equal(employeeArrivals.Length, model.Length);

            Assert.Equal(employeeArrivals, model);
        }

        // Test with nameSearch and arrivalDateSearch parameters
        [Fact]
        public async Task Index_WithNameSearchAndArrivalDateSearch_ReturnsOkObjectResult()
        {
            var controller = new EmployeeArrivalsReaderController(_context);

            string nameSearch = "Dan";
            DateTime arrivalDateSearch = DateTime.Today.AddDays(-1);

            IActionResult result = await controller.Index(null, nameSearch, arrivalDateSearch);

            var viewResult = Assert.IsType<OkObjectResult>(result);

            var model = Assert.IsAssignableFrom<EmployeeArrival[]>(viewResult.Value);
            var employeeArrivals = await _context.EmployeeArrivals.Where(a => a.Employee.Name == nameSearch && a.ArrivalDateTime == arrivalDateSearch).ToArrayAsync();

            Assert.Equal(employeeArrivals.Length, model.Length);

            Assert.Equal(employeeArrivals, model);
        }

        // Test with all three parameters
        [Fact]
        public async Task Index_WithSortOrderNameSearchAndArrivalDateSearch_ReturnsOkObjectResult()
        {
            var controller = new EmployeeArrivalsReaderController(_context);

            string sortOrder = "name_desc";
            string nameSearch = "Dan";
            DateTime arrivalDateSearch = DateTime.Today.AddDays(-1);

            IActionResult result = await controller.Index(sortOrder, nameSearch, arrivalDateSearch);

            var viewResult = Assert.IsType<OkObjectResult>(result);

            var model = Assert.IsAssignableFrom<EmployeeArrival[]>(viewResult.Value);
            var employeeArrivals = await _context.EmployeeArrivals.Where(a => a.Employee.Name == nameSearch && a.ArrivalDateTime == arrivalDateSearch).OrderByDescending(a => a.Employee.Name).ToArrayAsync();

            Assert.Equal(employeeArrivals.Length, model.Length);

            Assert.Equal(employeeArrivals, model);
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}