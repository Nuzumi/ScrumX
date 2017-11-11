using ScrumX.API.Context;
using ScrumX.API.Interfaces;
using ScrumX.API.Model;
using ScrumX.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Logic
{
    public class UserRepo : IUserRepo
    {
        EfDbContext ctx;
        HistoryJobRepo hjRepo;

        public UserRepo(EfDbContext ctx)
        {
            this.ctx = ctx;
            hjRepo = new HistoryJobRepo(ctx);
        }

        public IEnumerable<User> Users
        {
            get { return ctx.Users.ToList(); }
        }


        public bool UserExists(string name)
        {
            return Users.ToList().Exists(U => U.Name.Equals(name));
        }

        public User GetUserByName(string name)
        {
            return Users.ToList().SingleOrDefault(U => U.Name.Equals(name));
        }

        public User GetUserById(int idUser)
        {
            return Users.ToList().SingleOrDefault(U => U.IdUser == idUser);
        }

        /// <summary>
        /// Logowanie usera
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns>
        /// 0 - użytkownik nie istnieje
        /// -1 - błędne hasło
        /// 1 - spoko
        /// </returns>
        public int UserLogin(string name, string password)
        {
            if (UserExists(name))
            {
                if (GetUserByName(name).Password.Equals(password))
                    return 1;
                else
                    return -1;
            }
            else
                return 0;
        }

        /// <summary>
        /// Zwraca true w przypadku jak sie zarejestruje, false w momencie gdy nick jest zajety
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool RegisterUser(string name, string password)
        {
            if (!UserExists(name))
            {
                ctx.Set<User>().Add(new User { Name = name, Password = password });
                ctx.SaveChanges();
                return true;
            }
            else return false;
        }

        public void DeleteUser(User obj)
        {
            hjRepo.DeleteHistoryJobForUser(obj);
            ctx.Set<User>().Remove(obj);
            ctx.SaveChanges();
        }

        public void EditUser(User obj)
        {
            ctx.Entry<User>(obj).CurrentValues.SetValues(obj);
            ctx.SaveChanges();
        }
    }
}
