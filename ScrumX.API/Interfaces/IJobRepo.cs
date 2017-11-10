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
    public interface IJobRepo
    {

         IEnumerable<Job> Jobs
        {
            get;
        }

         IEnumerable<Job> GetJobsForSprint(int idSprint);
         int AddJob(Job job);
         IEnumerable<Job> GetJobsInBacklog(Project project, int backlogStatus);
         IEnumerable<Job> GetJobsInTable(int sprint, int tableStatus);
         void DeleteJob(Job obj);
         Job ChangeJobSP(Job obj, int SP, User user);
         void EndJob(Job job, User user);
         bool EditJob(Job obj);
         bool ChangeJobTable(Job job, User user, int table);
    }
}
