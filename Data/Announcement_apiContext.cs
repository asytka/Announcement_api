using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Announcement_api.Model;

namespace Announcement_api.Data
{
    public class Announcement_apiContext : DbContext
    {
        public Announcement_apiContext (DbContextOptions<Announcement_apiContext> options)
            : base(options)
        {
        }

        public DbSet<Announcement_api.Model.Announcement> Announcement { get; set; } = default!;
    }
}
