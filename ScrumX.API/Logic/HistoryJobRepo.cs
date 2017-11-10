﻿using ScrumX.API.Context;
using ScrumX.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Repository
{
    public class HistoryJobRepo : IHistoryJobRepo
    {
        EfDbContext ctx;

        public HistoryJobRepo(EfDbContext ctx)
        {
            this.ctx = ctx;
        }

        public IEnumerable<HistoryJob> HistoryJobs
        {
            get { return ctx.HistoryJobs.ToList(); }
        }
        

        public int AddHistoryJob(HistoryJob hj)
        {
            ctx.Set<HistoryJob>().Add(hj);
            ctx.SaveChanges();
            int id = hj.IdHistoryJob;
            return id;
        }

        public IEnumerable<HistoryJob> GetHistoryJobsForJob(Job job)
        {
            return HistoryJobs.Where(h => h.IdJob == job.IdJob).ToList();
        }

        public bool AddHistoryJob(string comment, Job job, User user)
        {
            HistoryJob hj = new HistoryJob
            {
                Date = DateTime.Today,
                IdUser = user.IdUser,
                IdJob = job.IdJob
            };
            if (comment == null || comment.Equals(""))
                return false;
            else
                return AddHistoryJob(hj) > 0;
        }
        

        public void DeleteHistoryJob(HistoryJob obj)
        {
            ctx.Set<HistoryJob>().Remove(obj);
            ctx.SaveChanges();
        }
        
        public void EditHistoryJob(HistoryJob obj)
        {
            ctx.Entry<HistoryJob>(obj).CurrentValues.SetValues(obj);
            ctx.SaveChanges(); ;
        }

    }
}
