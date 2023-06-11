using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeArrivalModels
{
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmployeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SurName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Employee)obj;
            return Name == other.Name && SurName == other.SurName && Email == other.Email && Age == other.Age;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = (hash * 23) + (Name?.GetHashCode() ?? 0);
            hash = (hash * 23) + (SurName?.GetHashCode() ?? 0);
            hash = (hash * 23) + (Email?.GetHashCode() ?? 0);
            hash = (hash * 23) + Age.GetHashCode();
            return hash;
        }
    }
}