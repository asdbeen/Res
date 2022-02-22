using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Res.DbContexts
{
    public class ResDbContextFactory
    {
        private readonly string _connectionString;

        public ResDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ResDbContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;

            return new ResDbContext(options);
        }
    }
}
