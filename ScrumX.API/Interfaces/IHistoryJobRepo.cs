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

         void DeleteHistoryJob(HistoryJob obj);
        
         void EditHistoryJob(HistoryJob obj);

    }
}
