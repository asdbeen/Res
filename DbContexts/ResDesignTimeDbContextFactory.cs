using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Res.DbContexts
{
    public class ResDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ResDbContext>
    {

        public ResDbContext CreateDbContext(string[] args)
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite("Data Source = res.db").Options;

            return new ResDbContext(options);
        }
    }
}
