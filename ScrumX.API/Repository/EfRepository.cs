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

        public List<UsersProject> UsersProjects
        {
            get { return ctx.UsersProjects.ToList(); }
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

    }
}
