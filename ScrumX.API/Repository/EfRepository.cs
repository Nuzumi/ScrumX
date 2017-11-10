using Ninject;
using ScrumX.API.Context;
using ScrumX.API.Interfaces;
using ScrumX.API.Logic;
using ScrumX.API.Model;
using ScrumX.API.Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Repository
{
    public class EfRepository
    {
        IKernel kernel = new StandardKernel(new RepoModule());
        
        EfDbContext ctx = new EfDbContext();

        public IHistoryJobRepo HistoryJobsRepo { get; set; }
        public IJobRepo JobsRepo { get; set; }
        public IUserRepo UsersRepo { get; set; }
        public IProjectRepo ProjectsRepo { get; set; }
        public ISprintRepo SprintsRepo { get; set; }

        public EfRepository()
        {
            HistoryJobsRepo = kernel.Get<IHistoryJobRepo>();
            JobsRepo = kernel.Get<IJobRepo>();
            UsersRepo = kernel.Get<IUserRepo>();
            SprintsRepo = kernel.Get<ISprintRepo>();
            ProjectsRepo = kernel.Get<IProjectRepo>();

        }

        public void SaveChanges()
        {
            ctx.SaveChanges();
        }
    }
}
