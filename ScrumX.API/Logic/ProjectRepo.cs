﻿using ScrumX.API.Context;
using ScrumX.API.Model;
using ScrumX.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Logic
{
    public class ProjectRepo : IProjectRepo
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
            int id = ctx.Set<Project>().Add(project).IdProject;
            ctx.SaveChanges();
            return id;
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
            ctx.SaveChanges();
        }
        
        public void EditProject(Project obj)
        {
            if (obj.Name != null && obj.Name.Equals(""))
            {
                ctx.Entry<Project>(obj).CurrentValues.SetValues(obj);
                ctx.SaveChanges();
            }
        }

    }
}
