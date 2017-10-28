using ScrumX.API.Context;
using ScrumX.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Repository
{
    public class EfRepository
    {
        private EfDbContext ctx = new EfDbContext();

        public List<User> Users
        {
            get { return ctx.Users.ToList(); }
        }
        
        public List<Project> Projects
        {
            get { return ctx.Projects.ToList(); }
        }

        public List<Sprint> Sprints
        {
            get { return ctx.Sprints.ToList(); }
        }

        public List<Job> Jobs
        {
            get { return ctx.Jobs.ToList(); }
        }

        public List<HistoryJob> HistoryJobs
        {
            get { return ctx.HistoryJobs.ToList(); }
        }

        public bool UserExists(string name) {
            return Users.Exists(U => U.Name.Equals(name));
        }

        public User GetUserByName(string name)
        {
            return Users.SingleOrDefault(U => U.Name.Equals(name));
        }

        public User GetUserById(int idUser)
        {
            return Users.SingleOrDefault(U => U.IdUser == idUser);
        }

        public string UserLogin(string name, string password) {
            if (UserExists(name))
            {
                if (GetUserByName(name).Password.Equals(password))
                    return "Zalogowano";
                else
                    return "Błędne hasło";
            }
            else
                return "Użytkownik nie istnieje";
        }

        public List<Sprint> GetSprintsForProject(int idProject)
        {
            return Sprints.Where(S => S.IdProject == idProject).ToList();
        }

        public List<Job> GetJobsForSprint(int idSprint)
        {
            return Jobs.Where(J => J.IdSprint == idSprint).ToList();
        }
        /// <summary>
        /// Zwraca true w przypadku jak sie zarejestruje, false w momencie gdy nick jest zajety
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string RegisterUser(string name, string password)
        {
            if (!UserExists(name))
            {
                ctx.Set<User>().Add(new User { Name = name, Password = password });
                return "Dodano";
            }
            else return "Uzytkownik o danym nicku istnieje w bazie";
        }

        /// <summary>
        /// Zwraca id nowego zadania
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public int AddJob(Job job)
        {
            return ctx.Set<Job>().Add(job).IdJob;
        }

        /// <summary>
        /// Bez numeru i tytułu, sprintu, wyliczalny
        /// </summary>
        /// <param name="sprint"></param>
        /// <returns></returns>
        public int AddSprint(Sprint sprint)
        {
            List<Sprint> sprints = GetSprintsForProject(sprint.IdProject);
            sprint.NoSprint = sprints.Count == 0 ? 1 : sprints.Max(q => q.NoSprint) + 1;
            sprint.Title = "Sprint " + sprint.NoSprint;
            return ctx.Set<Sprint>().Add(sprint).IdSprint;
        }

        /// <summary>
        /// Bez daty poczatkowej
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public int AddProject(Project project)
        {
            return ctx.Set<Project>().Add(project).IdProject;
        }

        public List<Job> GetJobsInBacklog(int sprint, int backlogStatus)
        {
            return Jobs.Where(J=> J.IdSprint == sprint).Where(J => J.BacklogStatus == backlogStatus).ToList();
        }

        public List<Job> GetJobsInTable(int sprint, int tableStatus)
        {
            return Jobs.Where(J => J.IdSprint == sprint).Where(J => J.TableStatus == tableStatus).ToList();
        }



    }
}
