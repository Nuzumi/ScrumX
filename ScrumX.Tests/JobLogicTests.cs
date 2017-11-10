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
    public class JobLogicTests
    {

        EfRepository repo;

        [TestInitialize]
        public void BeforeTest()
        {
            repo = new EfRepository();
            Console.WriteLine("BeforeTest");
        }

        [TestCleanup]
        public void AfterTest()
        {
            Console.WriteLine("AfterTest");
        }

        [TestMethod]
        public void GetJobs()
        {
            Assert.IsNotNull(repo.JobsRepo.Jobs);
        }
        
        [TestMethod]
        public void SearchJob()
        {
            Assert.AreEqual(repo.JobsRepo.SearchJob("test").Count(),1);            
        }

        [TestMethod]
        public void AddJob()
        {
            repo.JobsRepo.AddJob(new Job { Title = "Test zadanie", IdProject = 6, IdUser = 1, Priority = 0, IdSprint = 0});
            Assert.AreEqual(repo.JobsRepo.GetJobById(repo.JobsRepo.Jobs.Max(j => j.IdJob)).Title, "Test zadanie");
        }

        [TestMethod]
        public void ChangeJobPriority()
        {
            
        }
    }
}
