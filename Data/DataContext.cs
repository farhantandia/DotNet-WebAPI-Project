using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Data
{
    public class DataContext :DbContext
    {private readonly string connectionString;
        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
        {
             connectionString = configuration.GetSection("ConnectionStrings:DefaultConnection").Value!;
    }
        

        public DbSet<Character> Characters => Set<Character>();
    }
}