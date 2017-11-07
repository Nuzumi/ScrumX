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
        public void UserLogin()
        {
            Mock<IUserRepo> mock = new Mock<IUserRepo>();
            mock.Setup(m => m.Users).Returns(new User[] {
                new User { Name = "Admin", Password = "Admin" }
            });
            mock.Setup(m => m.UserExists("Admin")).Returns(true);
            
            Assert.IsTrue(mock.Object.UserExists("Admin"));
        }
    }
}
