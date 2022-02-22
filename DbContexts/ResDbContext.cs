using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Res.Models;
using Res.DTOs;

namespace Res.DbContexts
{
    public class ResDbContext : DbContext
    {
        public ResDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ReservationDTO>  Reservations { get; set; } 
    }
}
