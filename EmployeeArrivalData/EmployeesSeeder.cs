using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EmployeeArrivalModels;

namespace EmployeeArrivalData
{
    public class EmployeesSeeder
    {
        public static void Seed(EmployeeArrivalContext context, string jsonFilePath)
        {
            if (!context.Employees.Any())
            {
                var jsonData = File.ReadAllText(jsonFilePath);
                var jsonEmployeesData = JsonSerializer.Deserialize<List<JsonEmployee>>(jsonData);

                if (jsonEmployeesData != null)
                {
                    var employeesData =
                        from e in jsonEmployeesData
                        select new Employee()
                        {
                            EmployeeId = e.Id,
                            Name = e.Name,
                            SurName = e.SurName,
                            Email = e.Email,
                            Age = e.Age
                        };

                    context.Employees.AddRange(employeesData);
                    context.SaveChanges();
                }
            }
        }
    }
}
