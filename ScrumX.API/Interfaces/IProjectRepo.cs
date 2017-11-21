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
    public interface IProjectRepo
    {
         IEnumerable<Project> Projects
        {
            get;
        }

        Project FindProjectByName(string name);
         int AddProject(Project project);

         bool AddProject(string name);


         void DeleteProject(Project obj);

         void EditProject(Project obj);
        void ChangeName(Project project, string name);
    }
}
