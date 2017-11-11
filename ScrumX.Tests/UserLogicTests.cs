using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ScrumX.API.Interfaces;
using ScrumX.API.Logic;
using ScrumX.API.Model;
using ScrumX.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.Tests
{
    [TestClass]
    public class UserLogicTests
    {
        [TestMethod]
        public void UserExists()
        {
            EfRepository repo = new EfRepository();
            
            Assert.IsTrue(repo.UsersRepo.UserExists("admin"));
            Assert.IsFalse(repo.UsersRepo.UserExists("Kasia"));
        }

        [TestMethod]
        public void UserLogin()
        {
            EfRepository repo = new EfRepository();

            Assert.AreEqual(repo.UsersRepo.UserLogin("admin", "admin"), 1);
            Assert.AreEqual(repo.UsersRepo.UserLogin("Roenna", "admin"), 0);
            Assert.AreEqual(repo.UsersRepo.UserLogin("admin", "ADMIN1"), -1);
        }

        [TestMethod]
        public void UserRegister()
        {
            EfRepository repo = new EfRepository();

            Assert.IsFalse(repo.UsersRepo.RegisterUser("admin", "admin"));
            
            Assert.IsTrue(repo.UsersRepo.RegisterUser("Admin1", "Admin1"));
            
        }

        [TestMethod]
        public void GetUserById()
        {
            EfRepository repo = new EfRepository();

            Assert.IsNotNull(repo.UsersRepo.GetUserById(1));
            Assert.IsNull(repo.UsersRepo.GetUserById(10542));

        }

        [TestMethod]
        public void GetUserByName()
        {
            EfRepository repo = new EfRepository();
            repo.UsersRepo.RegisterUser("Test", "Test");
            User user = repo.UsersRepo.GetUserByName("Test");
            Assert.IsNotNull(repo.UsersRepo.GetUserByName("Test"));
            Assert.IsNull(repo.UsersRepo.GetUserByName("Roenna"));

        }

        [TestMethod]
        public void EditUser()
        {
            EfRepository repo = new EfRepository();
            repo.UsersRepo.RegisterUser("Test", "Test");
            User user = repo.UsersRepo.GetUserByName("Test");
            user.Password = "pass";

            repo.UsersRepo.EditUser(user);

            Assert.IsTrue(repo.UsersRepo.GetUserByName("Test").Password.Equals("pass"));
        }

        [TestMethod]
        public void DeleteUser()
        {
            EfRepository repo = new EfRepository();
            repo.UsersRepo.RegisterUser("Test", "Test");
            User user = repo.UsersRepo.GetUserByName("Test");
            int id = user.IdUser;
            repo.UsersRepo.DeleteUser(user);

            Assert.IsNull(repo.UsersRepo.GetUserByName("Test"));
            Assert.AreEqual(repo.HistoryJobsRepo.GetHistoryJobsForUser(id).Count(), 0);
        }

        public void DeleteUser(string name)
        {
            EfRepository repo = new EfRepository();
            User user = repo.UsersRepo.GetUserByName(name);
            repo.UsersRepo.DeleteUser(user);
        }

    }
}
