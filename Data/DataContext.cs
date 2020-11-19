using API_Course.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Course.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options ): base(options)
        {
        
        }

        
            public DbSet<Character> Characters {get; set;}
             public DbSet<User> Users { get; set; }

            public DbSet<WeaponModel> Weapons {get; set;}

    }

}
