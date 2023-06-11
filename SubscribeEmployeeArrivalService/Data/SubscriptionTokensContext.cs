using Microsoft.EntityFrameworkCore;
using SubscribeEmployeeArrivalService.Models;

namespace SubscribeEmployeeArrivalService.Data
{
    public class SubscriptionTokensContext : DbContext
    {
        public SubscriptionTokensContext(DbContextOptions<SubscriptionTokensContext> options) : base(options)
        {
        }

        public DbSet<Token> Tokens { get; set; }
    }
}
