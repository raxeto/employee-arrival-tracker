using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeArrivalData
{
    public class JsonEmployee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SurName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}
