using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace bkk_crawler_hq.LocalData
{
    public class BKKinfoContext : DbContext
    {
        public DbSet<BKKInfo> Bkkinfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=bkkinfo.db");
        }
    }

    public class BKKInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string RouteType { get; set; }
    }

}
