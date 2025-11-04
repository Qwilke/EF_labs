using Microsoft.EntityFrameworkCore;

namespace EF_Activity01_NetFramework
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

        }
    }
}
