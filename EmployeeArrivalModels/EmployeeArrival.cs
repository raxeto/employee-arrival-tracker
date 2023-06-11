using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeArrivalModels
{
    public class EmployeeArrival
    {
        public int EmployeeArrivalId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public Employee Employee { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (EmployeeArrival)obj;
            return EmployeeArrivalId == other.EmployeeArrivalId
                && EmployeeId == other.EmployeeId
                && ArrivalDateTime == other.ArrivalDateTime
                && (Employee?.Equals(other.Employee) ?? other.Employee == null);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = (hash * 23) + EmployeeArrivalId.GetHashCode();
            hash = (hash * 23) + EmployeeId.GetHashCode();
            hash = (hash * 23) + ArrivalDateTime.GetHashCode();
            hash = (hash * 23) + (Employee?.GetHashCode() ?? 0);
            return hash;
        }
    }
}
