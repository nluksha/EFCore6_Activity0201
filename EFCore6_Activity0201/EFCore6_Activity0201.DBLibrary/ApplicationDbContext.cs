using Microsoft.EntityFrameworkCore;

namespace EFCore6_Activity0201.DBLibrary
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}