using ScrumX.API.Context;
using ScrumX.API.Model;
using ScrumX.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Logic
{
    public class SprintRepo
    {
        EfDbContext ctx;
        ProjectRepo projectRepo;

        public SprintRepo(EfDbContext ctx)
        {
            this.ctx = ctx;
            projectRepo = new ProjectRepo(ctx);
        }

        public IEnumerable<Sprint> Sprints
        {
            get { return ctx.Sprints.ToList(); }
        }




        public IEnumerable<Sprint> GetSprintsForProject(int idProject)
        {
            return Sprints.Where(S => S.IdProject == idProject).ToList();
        }



        /// <summary>
        /// Bez numeru i tytułu, sprintu, wyliczalny
        /// </summary>
        /// <param name="sprint"></param>
        /// <returns></returns>
        public int AddSprint(Project project)
        {
            Sprint sprint = new Sprint();
            IEnumerable<Sprint> sprints = GetSprintsForProject(project.IdProject);
            sprint.NoSprint = sprints.ToList().Count == 0 ? 1 : sprints.Max(q => q.NoSprint) + 1;
            sprint.Title = projectRepo.Projects.SingleOrDefault(P => P.IdProject == sprint.IdProject).Name + " - Sprint " + sprint.NoSprint;
            return ctx.Set<Sprint>().Add(sprint).IdSprint;
        }

        public void EditSprint(Sprint obj)
        {
            ctx.Entry<Sprint>(obj).CurrentValues.SetValues(obj);
        }
        
        public void DeleteSprint(Sprint obj)
        {
            ctx.Set<Sprint>().Remove(obj);
        }

    }
}
