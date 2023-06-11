using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using EmployeeArrivalModels;

namespace EmployeeArrivalData
{
    public class EmployeeArrivalContext : DbContext
    {
        public EmployeeArrivalContext(DbContextOptions<EmployeeArrivalContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeArrival> EmployeeArrivals { get; set; }
    }
}