using ScrumX.API.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Context
{
    public class EfDbContext : DbContext
    {
        //public static string connString = "Data Source=MICHAŁ-KOMPUTER\\SQLEXPRESS;Initial Catalog=Scrum;Integrated Security=True; App=EntityFramework";
        public static string connString = "Data Source=LAPTOP-9BBKLGE9\\MILENA;Initial Catalog=Scrum;Integrated Security=True; App=EntityFramework";

        public EfDbContext()
        : base(connString)
        {
        }

        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<Job> Jobs { get; set; }
        public virtual IDbSet<HistoryJob> HistoryJobs { get; set; }
        public virtual IDbSet<Project> Projects { get; set; }
        public virtual IDbSet<Sprint> Sprints { get; set; }
    }
}
