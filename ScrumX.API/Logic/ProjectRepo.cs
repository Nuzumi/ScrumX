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
    public class ProjectRepo
    {
        EfDbContext ctx;

        public ProjectRepo(EfDbContext ctx)
        {
            this.ctx = ctx;
        }
        
        public IEnumerable<Project> Projects
        {
            get { return ctx.Projects.ToList(); }
        }


        public int AddProject(Project project)
        {
            return ctx.Set<Project>().Add(project).IdProject;
        }

        public bool AddProject(string name)
        {
            Project project = new Project
            {
                Name = name,
                DayCreated = DateTime.Today
            };
            return AddProject(project) > 0;
        }


        public void DeleteProject(Project obj)
        {
            ctx.Set<Project>().Remove(obj);
        }
        
        public void EditProject(Project obj)
        {
            if(obj.Name != null && obj.Name.Equals(""))
                ctx.Entry<Project>(obj).CurrentValues.SetValues(obj);
        }

    }
}
