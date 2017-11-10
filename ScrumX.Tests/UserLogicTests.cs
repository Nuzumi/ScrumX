using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScrumX.API.Repository;
using Moq;
using ScrumX.API.Model;
using ScrumX.API.Logic;
using ScrumX.API.Interfaces;

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

            Assert.IsTrue(repo.UsersRepo.UserLogin("admin", "admin"));
            Assert.IsFalse(repo.UsersRepo.UserLogin("Admin", "admin"));
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
            repo.UsersRepo.RegisterUser("Admin1", "Admin1");
            Assert.IsNotNull(repo.UsersRepo.GetUserByName("Admin1"));
            Assert.IsNull(repo.UsersRepo.GetUserByName("Roenna"));

        }

        [TestMethod]
        public void EditUser()
        {
            EfRepository repo = new EfRepository();
            User user = repo.UsersRepo.GetUserByName("Admin1");
            user.Password = "pass";

            repo.UsersRepo.EditUser(user);

            Assert.IsTrue(repo.UsersRepo.GetUserByName("Admin1").Password.Equals("pass"));
        }

        [TestMethod]
        public void DeleteUser()
        {
            EfRepository repo = new EfRepository();
            User user = repo.UsersRepo.GetUserByName("Admin1");

            repo.UsersRepo.DeleteUser(user);
            Assert.IsNull(repo.UsersRepo.GetUserByName("Admin1"));
        }
        
    }
}
