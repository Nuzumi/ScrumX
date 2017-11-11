using ScrumX.API.Context;
using ScrumX.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Repository
{
    public interface IHistoryJobRepo
    {

         IEnumerable<HistoryJob> HistoryJobs
        {
            get;
        }


         int AddHistoryJob(HistoryJob hj);

         bool AddHistoryJob(string comment, Job job, User user);

        IEnumerable<HistoryJob> GetHistoryJobsForJob(Job job);
        IEnumerable<HistoryJob> GetHistoryJobsForJob(int job);

        IEnumerable<HistoryJob> GetHistoryJobsForUser(User user);
        IEnumerable<HistoryJob> GetHistoryJobsForUser(int user);

        void DeleteHistoryJob(HistoryJob obj);
        void DeleteHistoryJobForJob(Job obj);
            void DeleteHistoryJobForUser(User obj);


         void EditHistoryJob(HistoryJob obj);

    }
}
