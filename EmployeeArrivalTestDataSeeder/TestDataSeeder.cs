using EmployeeArrivalData;
using EmployeeArrivalModels;
using System.IO;

namespace EmployeeArrivalTestDataSeeder
{
    public class TestDataSeeder
    {
        public static void SeedData(EmployeeArrivalContext dbContext)
        {
            List<Employee> testEmployees = new List<Employee>(10);

            testEmployees.Add(new Employee() { EmployeeId = 0, Name = "Dan", SurName = "Smith", Age = 45, Email = "dan.smith@fake.com" });
            testEmployees.Add(new Employee() { EmployeeId = 1, Name = "Dan", SurName = "Cole", Age = 34, Email = "dan.Cole@fake.com" });
            testEmployees.Add(new Employee() { EmployeeId = 2, Name = "Math", SurName = "Smith", Age = 19, Email = "math.smith@fake.com" });
            testEmployees.Add(new Employee() { EmployeeId = 3, Name = "Dean", SurName = "Taylor", Age = 34, Email = "dean.taylor@fake.com" });
            testEmployees.Add(new Employee() { EmployeeId = 4, Name = "Jonathan", SurName = "Barby", Age = 50, Email = "jonathan.barby@fake.com" });
            testEmployees.Add(new Employee() { EmployeeId = 5, Name = "Nick", SurName = "Cave", Age = 24, Email = "nick.cave@fake.com" });
            testEmployees.Add(new Employee() { EmployeeId = 6, Name = "Rebecka", SurName = "Jones", Age = 26, Email = "rebecka.jones@fake.com" });
            testEmployees.Add(new Employee() { EmployeeId = 7, Name = "Jenny", SurName = "Cole", Age = 27, Email = "jenny.cole@fake.com" });
            testEmployees.Add(new Employee() { EmployeeId = 8, Name = "Anna", SurName = "Brown", Age = 31, Email = "anna.brown@fake.com" });
            testEmployees.Add(new Employee() { EmployeeId = 9, Name = "Nia", SurName = "Bush", Age = 38, Email = "nia.bush@fake.com" });

            dbContext.Employees.AddRange(testEmployees);

            Random random = new Random();
            DateTime endDate = DateTime.Today;

            foreach (var employee in testEmployees)
            {
                for (int i = 0; i < 3; ++i)
                {
                    dbContext.EmployeeArrivals.Add(new EmployeeArrival() { EmployeeId = employee.EmployeeId, ArrivalDateTime = endDate.AddDays((-1) * random.Next(10)), Employee = employee });
                }
            }

            dbContext.SaveChanges();
        }

        public static string GetTestFilePath(string baseDirectory)
        {
            // TODO: Find a more elegant way to do it
            var seedFileDir = new DirectoryInfo(baseDirectory);

            while (seedFileDir.Name != "EmployeeArrivalTracker")
            {
                // Traverse up the directory tree until we find the base solution directory
                seedFileDir = seedFileDir.Parent;

                if (seedFileDir == null)
                {
                    throw new Exception("Solution directory not found");
                }
            }

            return  Path.Combine(seedFileDir.FullName, "EmployeeArrivalTestDataSeeder\\employees.json");
        }
    }
}