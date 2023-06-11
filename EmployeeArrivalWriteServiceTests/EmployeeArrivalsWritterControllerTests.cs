using EmployeeArrivalData;
using EmployeeArrivalModels;
using EmployeeArrivalTestDataSeeder;
using EmployeeArrivalWriteService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeArrivalWriteServiceTests
{
    public class EmployeeArrivalsWritterControllerTests : IDisposable
    {
        private readonly EmployeeArrivalContext _context;

        public EmployeeArrivalsWritterControllerTests()
        {
            var options = new DbContextOptionsBuilder<EmployeeArrivalContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new EmployeeArrivalContext(options);
            _context.Database.EnsureCreated();

            TestDataSeeder.SeedData(_context);
        }

        [Fact]
        public async Task Submit_WithValidInput_ReturnsOkObjectResult()
        {
            var controller = new EmployeeArrivalsWritterController(_context);

            JsonEmployeeArrival[] arrivalsToSave = new JsonEmployeeArrival[3];

            arrivalsToSave[0] = new JsonEmployeeArrival() { EmployeeId = 0, When = DateTime.Now };
            arrivalsToSave[1] = new JsonEmployeeArrival() { EmployeeId = 1, When = DateTime.Now };
            arrivalsToSave[2] = new JsonEmployeeArrival() { EmployeeId = 2, When = DateTime.Now };

            int arrivalBeforeSave = await _context.EmployeeArrivals.CountAsync();

            IActionResult result = await controller.Submit(arrivalsToSave);

            var viewResult = Assert.IsType<OkObjectResult>(result);

            int arrivalsAfterSave = await _context.EmployeeArrivals.CountAsync();

            Assert.Equal(arrivalBeforeSave + arrivalsToSave.Length, arrivalsAfterSave);

            for (int i = 0; i < arrivalsToSave.Length; ++i)
            {
                var savedArrival = _context.EmployeeArrivals.Where(a => a.EmployeeId == arrivalsToSave[i].EmployeeId && a.ArrivalDateTime == arrivalsToSave[i].When).FirstOrDefault();

                Assert.NotNull(savedArrival);
            }
        }

        [Fact]
        public async Task Submit_WithBadDataInput_ReturnsBadRequest()
        {
            var controller = new EmployeeArrivalsWritterController(_context);
            
            controller.ModelState.AddModelError("Error", "Bad state");

            JsonEmployeeArrival[] arrivalsToSave = new JsonEmployeeArrival[3];

            arrivalsToSave[0] = new JsonEmployeeArrival() { EmployeeId = 0, When = DateTime.Now };
            arrivalsToSave[1] = new JsonEmployeeArrival() { EmployeeId = 1, When = DateTime.Now };
            arrivalsToSave[2] = new JsonEmployeeArrival() { EmployeeId = 2, When = DateTime.Now };

            int arrivalBeforeSave = await _context.EmployeeArrivals.CountAsync();

            IActionResult result = await controller.Submit(arrivalsToSave);

            var viewResult = Assert.IsType<BadRequestObjectResult>(result);

            int arrivalsAfterSave = await _context.EmployeeArrivals.CountAsync();

            Assert.Equal(arrivalBeforeSave, arrivalsAfterSave);
        }

        [Fact]
        public async Task Submit_WithNullDataInput_ReturnsBadRequest()
        {
            var controller = new EmployeeArrivalsWritterController(_context);

            int arrivalBeforeSave = await _context.EmployeeArrivals.CountAsync();

            IActionResult result = await controller.Submit(null);

            var viewResult = Assert.IsType<BadRequestObjectResult>(result);

            int arrivalsAfterSave = await _context.EmployeeArrivals.CountAsync();

            Assert.Equal(arrivalBeforeSave, arrivalsAfterSave);
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}