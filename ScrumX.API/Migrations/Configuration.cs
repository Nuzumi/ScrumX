namespace ScrumX.API.Migrations
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ScrumX.API.Context.EfDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ScrumX.API.Context.EfDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Projects.AddOrUpdate(
                p => p.IdProject,
                new Project
                {
                    Name = "Project1",
                    DayCreated = DateTime.Today,
                    ProjectSprint = new List<Sprint> { new Sprint { Title = "Sprint 1", StartData = DateTime.Today,
                        SprintJob = new List<Job> { new Job { Title = "Ogarnac baze", Desc = "Mocno", User = new User { Name = "admin", Password = "admin" } } } } }
                }
                );
        }
    }
}
