using System.ComponentModel.DataAnnotations;

namespace SubscribeEmployeeArrivalService.Models
{
    public class Token
    {
        [Key]
        public string TokenValue { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
    }
}
