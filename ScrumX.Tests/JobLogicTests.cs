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
            Assert.AreEqual(repo.JobsRepo.SearchJob("Test").Count(),1);
            AfterTest();         
        }

        [TestMethod]
        public void AddJob()
        {
            int id = repo.JobsRepo.AddJob(new Job { Title = "Test zadanie", IdProject = 6, IdUser = 1, Priority = 0, IdSprint = 6});
            Assert.AreNotEqual(repo.JobsRepo.GetJobById(id),0);
            Assert.IsNotNull(repo.HistoryJobsRepo.GetHistoryJobsForJob(repo.JobsRepo.GetJobById(id)));
        }

        public int AddJob(Job job)
        {
            return repo.JobsRepo.AddJob(job);
        }

        [TestMethod]
        public void ChangeJobSP()
        {
            int id = AddJob(new Job { Title = "Test zadanie", IdProject = 6, IdUser = 1, Priority = 0, IdSprint = 6 });

            repo.JobsRepo.ChangeJobSP(repo.JobsRepo.GetJobById(id), 8, repo.UsersRepo.GetUserById(1));

            Assert.IsTrue(repo.JobsRepo.GetJobById(id).SP == 8);
            Assert.IsNotNull(repo.HistoryJobsRepo.GetHistoryJobsForJob(repo.JobsRepo.GetJobById(id))
                .Where(p => p.IdJob == id)
                .Where(p => p.ToBacklog == 2)
                .Where(p=>p.ToTable == 1).SingleOrDefault());

            DeleteJob(id);
        }

        [TestMethod]
        public void ChangeJobPriority()
        {
            int id = AddJob(new Job { Title = "Test zadanie", IdProject = 6, IdUser = 1, Priority = 0, IdSprint = 6 });

            repo.JobsRepo.ChangeJobPriority(repo.JobsRepo.GetJobById(id), 8, repo.UsersRepo.GetUserById(1));

            Assert.IsTrue(repo.JobsRepo.GetJobById(id).Priority == 8);

            repo.JobsRepo.ChangeJobPriority(repo.JobsRepo.GetJobById(id), 87445, repo.UsersRepo.GetUserById(1));

            Assert.IsTrue(repo.JobsRepo.GetJobById(id).Priority == 10);

            DeleteJob(id);
        }

        [TestMethod]
        public void DeleteJob()
        {
            int id = AddJob(new Job { Title = "Test zadanie", IdProject = 6, IdUser = 1, Priority = 0, IdSprint = 6 });
            repo.JobsRepo.DeleteJob(repo.JobsRepo.GetJobById(id));

            Assert.IsNull(repo.JobsRepo.GetJobById(id));

        }

        public void DeleteJob(int id)
        {
            repo.JobsRepo.DeleteJob(repo.JobsRepo.GetJobById(id));
        }

    }
}
