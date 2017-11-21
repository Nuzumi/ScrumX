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
         Job GetJobById(int id);
         IEnumerable<Job> GetJobsForSprint(Sprint sprint);
         IEnumerable<Job> GetJobsForProject(Project project);
         int AddJob(Job job);
         IEnumerable<Job> GetJobsInBacklog(Project project, int backlogStatus);
         IEnumerable<Job> GetJobsInTable(Sprint sprint, int tableStatus);
         void DeleteJob(Job obj);
        Job ChangeJobSP(Job obj, double SP, User user);
            Job ChangeJobPriority(Job obj, double? priority, User user);
         void EndJob(Job job, User user);
         bool EditJob(Job obj);
         bool ChangeJobTable(Job job, User user, int table);
        IEnumerable<Job> SearchJob(Project project, string tag);
        IEnumerable<Job> GetJobsForUser(Sprint sprint, User user, int table);
        void ChangeJobTitle(Job job, string Title);
        void ChangeJobSPWithoutLog(Job actualJob, double? sp);
        void CloseSprint(Sprint sprint);
        void ChangeJobPrority(Job actualJob, double? prior);

        void ChangeJobDesc(Job actualJob, string desc);
    }
}
