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

        Mock<IJobRepo> repo = new Mock<IJobRepo>();
        
        [TestMethod]
        public void Setup()
        {
            //repo.Setup(m => m.Jobs).Returns<List<Job>>()
        }

        [TestMethod]
        public void GetJobs()
        {
            Setup();
            Console.Write(repo.Object.Jobs.Count());
        }

        [TestMethod]
        public void SearchJob()
        {
            Setup();
            Assert.AreEqual(repo.Object.SearchJob("e").Count(),3);            
        }

        [TestMethod]
        public void AddJob()
        {
            Setup();
            repo.Object.AddJob(new Job { Title = "Test zadanie", IdProject = 6, IdUser = 1, Priority = 0 });
            Assert.AreEqual(repo.Object.GetJobById(repo.Object.Jobs.Max(j => j.IdJob)).Title, "Test zadanie");
        }

        [TestMethod]
        public void ChangeJobPriority()
        {
            
        }
    }
}
