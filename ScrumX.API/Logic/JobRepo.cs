using ScrumX.API.Content;
using ScrumX.API.Context;
using ScrumX.API.Model;
using ScrumX.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScrumX.API.Logic
{
    public class JobRepo : IJobRepo
    {
        EfDbContext ctx;
        HistoryJobRepo hjRepo;
        UserRepo userRepo;
        SprintRepo sprintRepo;

        public JobRepo(EfDbContext ctx)
        {
            this.ctx = ctx;
            hjRepo = new HistoryJobRepo(ctx);
            userRepo = new UserRepo(ctx);
            sprintRepo = new SprintRepo(ctx);
        }

        public IEnumerable<Job> Jobs
        {
            get { return ctx.Jobs.ToList(); }
        }

        public IEnumerable<Job> GetJobsForSprint(Sprint sprint)
        {
            return Jobs.Where(J => J.IdSprint == sprint.IdSprint).ToList();
        }

        public IEnumerable<Job> GetJobsForProject(Project project)
        {
            return Jobs.Where(J => J.IdProject == project.IdProject).ToList();
        }

        /// <summary>
        /// Zwraca id nowego zadania
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public int AddJob(Job job)
        {
            job.SP = job.SP == 0 ? null : job.SP;
            job.IdSprint = sprintRepo.GetLastSprintForProject(job.IdProject).IdSprint;
            ctx.Set<Job>().Add(job);
            ctx.SaveChanges();
            int id = job.IdJob;
            //W momencie utworzenia zadania tworzy sie log
            HistoryJob hj = new HistoryJob();
            hj.FromBacklog = (int)typeBacklog.New;
            hj.ToBacklog = (int)typeBacklog.New;
            hj.FromTable = (int)typeTable.None;
            hj.ToTable = (int)typeTable.None;
            hj.IdJob = job.IdJob;
            hj.IdUser = job.IdUser;
            hj.Date = DateTime.Today;
            hj.Comment = "Utworzono zadanie \"" + job.Title + "\" przez użytkownika " + userRepo.Users.SingleOrDefault(U => U.IdUser == job.IdUser).Name;

            ctx.SaveChanges();
            hjRepo.AddHistoryJob(hj);
            return id;
        }

        public Job GetJobById(int id)
        {
            return Jobs.Where(j => j.IdJob == id).SingleOrDefault();
        }

        public IEnumerable<Job> GetJobsInBacklog(Project project, int backlogStatus)
        {
            var list = Jobs.Where(J => J.IdProject == project.IdProject);
            if (list != null)
                return backlogStatus == 0 ? list.ToList() : list.Where(J => J.BacklogStatus == backlogStatus).ToList();
            else return null;
        }

        public IEnumerable<Job> GetJobsInTable(Sprint sprint, int tableStatus)
        {
            var list = Jobs.Where(J => J.IdSprint == sprint.IdSprint);
            if (list != null)
                return tableStatus == 0 ? list.ToList() : list.Where(J => J.TableStatus == tableStatus).ToList();
            else return null;
        }

        public void DeleteJob(Job obj)
        {
            hjRepo.DeleteHistoryJobForJob(obj);
            ctx.Set<Job>().Remove(obj);
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

            if (obj.BacklogStatus != (int)typeBacklog.Completed)
            {
                //Edit zadania robi wpis w HJ
                HistoryJob hj = new HistoryJob();
                hj.Date = DateTime.Today;
                hj.IdJob = obj.IdJob;
                hj.IdUser = user.IdUser;
                //W przypadku jak nigdy wczesniej nie mial SP
                if (obj.BacklogStatus == (int)typeBacklog.New) //czyli jest w New
                {
                    hj.FromBacklog = obj.BacklogStatus;
                    hj.ToBacklog = (int)typeBacklog.Ready; //Zmiana na Ready
                    obj.BacklogStatus = (int)typeBacklog.Ready;
                    hj.FromTable = obj.TableStatus;
                    hj.ToTable = (int)typeTable.ToDo; //do tablicy To-do
                    obj.TableStatus = (int)typeTable.ToDo;
                    obj.SP = SP;
                    hj.Comment = "Przeniesiono zadanie \"" + obj.Title + "\" do rejestru " + Enum.GetName(typeof(Content.typeBacklog), 2) + " przez użytkownika "
                        + user.Name + "\n";
                }
                else
                {
                    
                    hj.FromBacklog = hj.ToBacklog = obj.BacklogStatus;
                    hj.FromTable = hj.ToTable = obj.TableStatus;
                }
                hj.Comment = (hj.Comment == null ? "" : hj.Comment) + "Zmiana SP na " + SP + " przez użytkownika " + user.Name;
                obj.SP = SP;
                obj.IdSprint = sprintRepo.GetLastSprintForProject(obj.IdProject).IdSprint;
                hjRepo.AddHistoryJob(hj);
                EditJob(obj);
                return obj;
            }
            else
                return null;
        }


        /// <summary>
        /// Max priorytet = 10
        /// Nie mozna zmienic SP zadania, ktore jest Completed.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="SP"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Job ChangeJobPriority(Job obj, int priority, User user)
        {
            //Jak nie jest completed
            if (obj.BacklogStatus != 3)
            {
                //Edit zadania robi wpis w HJ
                HistoryJob hj = new HistoryJob();
                hj.Date = DateTime.Today;
                hj.IdJob = obj.IdJob;
                hj.IdUser = user.IdUser;
                int? pr = obj.Priority.HasValue ? obj.Priority.Value : 0;
                priority = priority > 10 ? 10 : priority;
                hj.Comment = "Zmiana priorytetu zadania " + obj.Title + " z " + pr + " na " + priority + " przez " + user.Name;
                obj.Priority = priority;
                hjRepo.AddHistoryJob(hj);
                EditJob(obj);
                return obj;
            }
            else
                return null;
        }


        public void EndJob(Job job, User user)
        {
            ChangeJobTable(job, user, (int)typeTable.Done);
            EditJob(job);
        }

        public bool EditJob(Job obj)
        {

            ctx.Entry<Job>(obj).CurrentValues.SetValues(obj);
            ctx.SaveChanges();
            return true;

        }

        public bool ChangeJobTable(Job job, User user, int table)
        {
            if (job.TableStatus != (int)typeTable.Done)
            {
                if (job.TableStatus == (int)typeTable.ToDo && table != (int)typeTable.Doing) return false;
                HistoryJob hj = new HistoryJob
                {
                    Date = DateTime.Today,
                    FromBacklog = job.BacklogStatus,
                    FromTable = job.TableStatus,
                    ToBacklog = job.BacklogStatus,
                    ToTable = table,
                    IdJob = job.IdJob,
                    IdUser = user.IdUser,
                    Comment = "Przeniesiono zadanie \"" + job.Title + "\" do rejestru " + Enum.GetName(typeof(Content.typeTable), table) + " przez użytkownika "
                        + user.Name
                };
                if (table == (int)typeTable.Done)
                {
                    hj.ToBacklog = (int)typeBacklog.Completed;
                    job.BacklogStatus = (int)typeBacklog.Completed;
                }
                if (table == (int)typeTable.Doing)
                {
                    hj.ToBacklog = (int)typeBacklog.Scheduled;
                    job.BacklogStatus = (int)typeBacklog.Scheduled;
                }
                if (table == (int)typeTable.ToDo)
                {
                    hj.ToBacklog = (int)typeBacklog.Ready;
                    job.BacklogStatus = (int)typeBacklog.Ready;
                }
                if (table == (int)typeTable.Review)
                {
                    hj.ToBacklog = (int)typeBacklog.Scheduled;
                    job.BacklogStatus = (int)typeBacklog.Scheduled;
                }
                hjRepo.AddHistoryJob(hj);
                job.TableStatus = table;
                return EditJob(job);
            }
            else return false;
        }

        public IEnumerable<Job> SearchJob(Project project, string tag)
        {
            var listJob = Jobs.Where(p => p.Title == "NA PEWNO NIE MA TAKIEGO ZADANIA");
            string[] split = tag.Split(' ');
            foreach (string st in split){
                if (st != "")
                {
                    var list = Jobs.Where(p => p.IdProject == project.IdProject).Where(p => p.Title.ToLower().Contains(st));
                    listJob = listJob.Union(list);
                }
            }
            return listJob;
        }

        public IEnumerable<Job> GetJobsForUser(Sprint sprint, User user, int table)
        {
            return Jobs    // your starting point - table in the "from" statement
                       .Join(hjRepo.HistoryJobs, // the source table of the inner join
                          p => p.IdJob,       // Select the primary key (the first part of the "on" clause in an sql "join" statement)
                          h => h.IdJob,  // Select the foreign key (the second part of the "on" clause)
                          (p, h) => new { Job = p, HistoryJob = h })// selection
                          .Where(h => h.HistoryJob.IdUser == user.IdUser)
                          .Where(r=>r.Job.TableStatus == table)
                          .Where(r=>r.Job.IdSprint == sprint.IdSprint)
                       .Select(r => r.Job)
                       .Distinct<Job>(); 

            /*return Jobs    // your starting point - table in the "from" statement
                       .Join(hjRepo.HistoryJobs, // the source table of the inner join
                          p => p.IdJob,       // Select the primary key (the first part of the "on" clause in an sql "join" statement)
                          h => h.IdJob,  // Select the foreign key (the second part of the "on" clause)
                          (p, h) => new { Job = p, HistoryJob = h })// selection
                          .Where(h => h.HistoryJob.IdUser == user.IdUser)
                          .Where(r => r.Job.TableStatus == table)
                          .GroupBy(r => r.HistoryJob.IdUser)
                          .First()
                       .Select(r => r.Job)
                       .Distinct<Job>(); */
        }
    }
    
}
