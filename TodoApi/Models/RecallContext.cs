using Microsoft.EntityFrameworkCore;

namespace TransportCanada.API3.Models
{
    public class RecallContext: DbContext
    {
        public RecallContext (DbContextOptions<RecallContext> options)
            :base(options)
        {

        }

        public DbSet<Recall> Recalls { get; set; }
    }
}
