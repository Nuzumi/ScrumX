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

        public UserRepo(EfDbContext ctx)
        {
            this.ctx = ctx;
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
            return Users.SingleOrDefault(U => U.Name.Equals(name));
        }

        public User GetUserById(int idUser)
        {
            return Users.SingleOrDefault(U => U.IdUser == idUser);
        }

        public bool UserLogin(string name, string password)
        {
            if (UserExists(name))
            {
                if (GetUserByName(name).Password.Equals(password))
                    return true;
                else
                    return false;
            }
            else
                return false;
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

        public void DeleteUser(User obj)
        {
            ctx.Set<User>().Remove(obj);
        }

        public void EditUser(User obj)
        {
            ctx.Entry<User>(obj).CurrentValues.SetValues(obj);
        }
        
        public void SaveChanges()
        {
            ctx.SaveChanges();
        }
    }
}
