using Ninject.Modules;
using ScrumX.API.Logic;
using ScrumX.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrumX.API.Context;
using ScrumX.API.Repository;

namespace ScrumX.API.Ninject
{
    public class RepoModule : NinjectModule
    {
        private EfDbContext ctx;

        public RepoModule()
        {
            ctx = new EfDbContext();
        }

        public override void Load()
        {
            Bind<IProjectRepo>().To<ProjectRepo>().InSingletonScope().WithConstructorArgument("ctx", ctx);
            Bind<ISprintRepo>().To<SprintRepo>().InSingletonScope().WithConstructorArgument("ctx", ctx);
            Bind<IJobRepo>().To<JobRepo>().InSingletonScope().WithConstructorArgument("ctx", ctx);
            Bind<IUserRepo>().To<UserRepo>().InSingletonScope().WithConstructorArgument("ctx", ctx);
            Bind<IHistoryJobRepo>().To<HistoryJobRepo>().InSingletonScope().WithConstructorArgument("ctx", ctx);
           
        }
    }
}
