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
    public class JobRepo
    {
        EfDbContext ctx;
        HistoryJobRepo hjRepo;
        UserRepo userRepo;
        
        public JobRepo(EfDbContext ctx)
        {
            this.ctx = ctx;
            hjRepo = new HistoryJobRepo(ctx);
            userRepo = new UserRepo(ctx);
        }

        public IEnumerable<Job> Jobs
        {
            get { return ctx.Jobs.ToList(); }
        }

        public IEnumerable<Job> GetJobsForSprint(int idSprint)
        {
            return Jobs.Where(J => J.IdSprint == idSprint).ToList();
        }

        /// <summary>
        /// Zwraca id nowego zadania
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public int AddJob(Job job)
        {
            int idJob = ctx.Set<Job>().Add(job).IdJob;
            //W momencie utworzenia zadania tworzy sie log
            HistoryJob hj = new HistoryJob();
            hj.FromBacklog = 0;
            hj.ToBacklog = 0;
            hj.FromTable = 0;
            hj.ToTable = 0;
            hj.IdJob = idJob;
            hj.IdUser = job.IdUser;
            hj.Date = DateTime.Today;
            hj.Comment = "Utworzono zadanie \"" + job.Title + "\" przez użytkownika " + userRepo.Users.SingleOrDefault(U => U.IdUser == job.IdUser).Name;
            hjRepo.AddHistoryJob(hj);
            return idJob;
        }

        public IEnumerable<Job> GetJobsInBacklog(int sprint, int backlogStatus)
        {
            var list = Jobs.Where(J => J.IdSprint == sprint);
            return backlogStatus == 0 ? list.ToList() : list.Where(J => J.BacklogStatus == backlogStatus).ToList();
        }

        public IEnumerable<Job> GetJobsInTable(int sprint, int tableStatus)
        {
            var list = Jobs.Where(J => J.IdSprint == sprint);
            return tableStatus == 0 ? list.ToList() : list.Where(J => J.TableStatus == tableStatus).ToList();
        }

        public void DeleteJob(Job obj)
        {
            ctx.Set<Job>().Remove(obj);
        }

        public void SaveChanges()
        {
            ctx.SaveChanges();
        }
        /// <summary>
        /// Nie mozna zmienic SP zadania, ktore jest Completed.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="SP"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Job ChangeJobSP(Job obj, int SP, User user)
        {
            //Jak nie jest completed

            if (obj.BacklogStatus != 3)
            {
                //Edit zadania robi wpis w HJ
                HistoryJob hj = new HistoryJob();
                hj.Date = DateTime.Today;
                hj.IdJob = obj.IdJob;
                hj.IdUser = user.IdUser;
                //W przypadku jak nigdy wczesniej nie mial SP
                if (obj.BacklogStatus == 1) //czyli jest w New
                {
                    hj.FromBacklog = obj.BacklogStatus;
                    hj.ToBacklog = 2; //Zmiana na Scheduled
                    obj.BacklogStatus = 2;
                    hj.FromTable = obj.TableStatus;
                    hj.ToTable = 1; //do tablicy To-do
                    obj.TableStatus = 1;
                    obj.SP = SP;
                    hj.Comment = "Przeniesiono zadanie \"" + obj.Title + "\" do rejestru " + " " + " przez użytkownika "
                        + userRepo.Users.SingleOrDefault(U => U.IdUser == user.IdUser).Name;
                }
                else
                {
                    hj.Comment = "Zmiana SP z " + obj.SP + " na " + SP + " przez uzytkownika " + userRepo.Users.SingleOrDefault(U => U.IdUser == user.IdUser).Name;
                    obj.SP = SP;
                }
                hjRepo.AddHistoryJob(hj);
                EditJob(obj);
                return obj;
            }
            else
                return null;
        }

        public void EndJob(Job job, User user)
        {
            HistoryJob hj = new HistoryJob
            {
                Date = DateTime.Today,
                FromBacklog = job.BacklogStatus,
                FromTable = job.TableStatus,
                ToBacklog = 3,
                ToTable = 3,
                IdJob = job.IdJob,
                IdUser = user.IdUser,
                Comment = "Przeniesiono zadanie \"" + job.Title + "\" do rejestru " + " " + " przez użytkownika "
                        + user.Name
            };
            hjRepo.AddHistoryJob(hj);
            job.TableStatus = 3;
            job.BacklogStatus = 3;
            EditJob(job);
        }

        public bool EditJob(Job obj)
        {
            if (obj.TableStatus == 3)
                return false;
            else
            {
                ctx.Entry<Job>(obj).CurrentValues.SetValues(obj);
                return true;
            }
        }

        public bool ChangeJobTable(Job job, User user, int table)
        {
            if (job.TableStatus != 3)
            {
                HistoryJob hj = new HistoryJob
                {
                    Date = DateTime.Today,
                    FromBacklog = job.BacklogStatus,
                    FromTable = job.TableStatus,
                    ToBacklog = job.BacklogStatus,
                    ToTable = table,
                    IdJob = job.IdJob,
                    IdUser = user.IdUser,
                    Comment = "Przeniesiono zadanie \"" + job.Title + "\" do rejestru " + " " + " przez użytkownika "
                        + user.Name
                };
                hjRepo.AddHistoryJob(hj);
                job.TableStatus = table;
                return EditJob(job);
            }
            else return false;
        }
    }
}
