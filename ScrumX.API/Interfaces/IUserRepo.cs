﻿using ScrumX.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Interfaces
{
    public interface IUserRepo
    {
        IEnumerable<User> Users { get; }

        bool UserExists(string name);

         User GetUserByName(string name);

         User GetUserById(int idUser);

         int UserLogin(string name, string password);
        
         bool RegisterUser(string name, string password);
         void DeleteUser(User obj);

         void EditUser(User obj);
    }
}
