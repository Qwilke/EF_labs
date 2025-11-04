using Microsoft.EntityFrameworkCore;

namespace EF_Activity001
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

        }
    }
}
