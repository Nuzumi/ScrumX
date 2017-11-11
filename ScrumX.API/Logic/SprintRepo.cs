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
    public class SprintRepo : ISprintRepo
    {
        EfDbContext ctx;

        public SprintRepo(EfDbContext ctx)
        {
            this.ctx = ctx;
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
        /// <returns>
        /// Id sprintu, -1 w przypadku istniejacego otwartego sprintu dla projektu</returns>
        public int AddSprint(Project project, DateTime data)
        {
            //Otwarty sprint
            if (GetLastSprintForProject(project) == null)
            {
                Sprint sprint = new Sprint
                {
                    IdProject = project.IdProject,
                    EndData = data
                };
                IEnumerable<Sprint> sprints = GetSprintsForProject(project.IdProject);
                sprint.NoSprint = sprints.ToList().Count == 0 ? 1 : sprints.Max(q => q.NoSprint) + 1;
                sprint.Title = project.Name + " - Sprint " + sprint.NoSprint;
                ctx.Set<Sprint>().Add(sprint);
                ctx.SaveChanges();
                int id = sprint.IdSprint;
                return id;
            }
            else return -1;
        }

        /// <summary>
        /// Brak mozliwosci otwarcia zamknietego sprintu 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns> IdSprintu/ -1 </returns>
        public int EditSprint(Sprint obj)
        {
            //nie można otworzyć sprintu
            if (Sprints.Where(s => s.IdSprint == obj.IdSprint).SingleOrDefault().EndData <= DateTime.Today)
                return -1;
            else
            {
                ctx.Entry<Sprint>(obj).CurrentValues.SetValues(obj);
                ctx.SaveChanges();
                return obj.IdSprint;
            }
        }

        public void EndSprint(Sprint sprint)
        {
            sprint.EndData = DateTime.Today;
            EditSprint(sprint);
        }
        
        public void DeleteSprint(Sprint obj)
        {
            ctx.Set<Sprint>().Remove(obj);
            ctx.SaveChanges();
        }

        public void DeleteSprintsForProject(Project obj)
        {
            var list = Sprints.Where(s => s.IdProject == obj.IdProject);
            foreach (Sprint sprint in list)
                DeleteSprint(sprint);
        }

        public Sprint GetLastSprintForProject(Project project)
        {
            return Sprints.Where(s => s.IdProject == project.IdProject)
                .Where(s => s.StartData <= DateTime.Today)
                .Where(s=>s.EndData > DateTime.Today) //starczy, zabezpieczenie -> zamykam sprint -> EndData.
                .SingleOrDefault();
        }
    }
}
