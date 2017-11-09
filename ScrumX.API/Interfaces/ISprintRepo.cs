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
    public interface ISprintRepo
    {
         IEnumerable<Sprint> Sprints
        {
            get;
        }


         IEnumerable<Sprint> GetSprintsForProject(int idProject);
         int AddSprint(Project project, DateTime data);
        Sprint GetLastSprintForProject(Project project);
        Sprint GetLastSprintForProject(int project);
         int EditSprint(Sprint obj);
        void CloseSprint(Sprint sprint);
        bool IsSprintOpen(Sprint sprint);
         void DeleteSprint(Sprint obj);

    }
}
