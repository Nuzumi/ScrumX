using ScrumX.API.Context;
using ScrumX.API.Interfaces;
using ScrumX.API.Logic;
using ScrumX.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Repository
{
    public class EfRepository
    {
        EfDbContext ctx = new EfDbContext();

        public HistoryJobRepo HistoryJobsRepo;
        public JobRepo JobsRepo;
        public UserRepo UsersRepo;
        public ProjectRepo ProjectsRepo;
        public SprintRepo SprintsRepo;

        public EfRepository()
        {
            HistoryJobsRepo = new HistoryJobRepo(ctx);
            JobsRepo = new JobRepo(ctx);
            UsersRepo = new UserRepo(ctx);
            ProjectsRepo = new ProjectRepo(ctx);
            SprintsRepo = new SprintRepo(ctx);

        }
    }
}
