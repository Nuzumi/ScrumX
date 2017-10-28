namespace ScrumX.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HistoryJobs",
                c => new
                    {
                        IdHistoryJob = c.Int(nullable: false, identity: true),
                        IdJob = c.Int(nullable: false),
                        IdUser = c.Int(nullable: false),
                        Comment = c.String(),
                        FromBacklog = c.Int(nullable: false),
                        ToBacklog = c.Int(nullable: false),
                        FromTable = c.Int(nullable: false),
                        ToTable = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdHistoryJob)
                .ForeignKey("dbo.Jobs", t => t.IdJob, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdJob)
                .Index(t => t.IdUser);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        IdJob = c.Int(nullable: false, identity: true),
                        IdSprint = c.Int(nullable: false),
                        IdUser = c.Int(nullable: false),
                        Title = c.String(),
                        Desc = c.String(),
                        Priority = c.Int(),
                        SP = c.Int(),
                        BacklogStatus = c.Int(nullable: false),
                        TableStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdJob)
                .ForeignKey("dbo.Sprints", t => t.IdSprint, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: false)
                .Index(t => t.IdSprint)
                .Index(t => t.IdUser)
                .Index(t => t.BacklogStatus)
                .Index(t => t.TableStatus);
            
            CreateTable(
                "dbo.Sprints",
                c => new
                    {
                        IdSprint = c.Int(nullable: false, identity: true),
                        IdProject = c.Int(nullable: false),
                        NoSprint = c.Int(nullable: false),
                        Title = c.String(),
                        StartData = c.DateTime(nullable: false),
                        EndData = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdSprint)
                .ForeignKey("dbo.Projects", t => t.IdProject, cascadeDelete: true)
                .Index(t => t.IdProject);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        IdProject = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DayCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdProject);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        IdUser = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 20),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.IdUser);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HistoryJobs", "IdUser", "dbo.Users");
            DropForeignKey("dbo.Jobs", "IdUser", "dbo.Users");
            DropForeignKey("dbo.Jobs", "IdSprint", "dbo.Sprints");
            DropForeignKey("dbo.Sprints", "IdProject", "dbo.Projects");
            DropForeignKey("dbo.HistoryJobs", "IdJob", "dbo.Jobs");
            DropIndex("dbo.Sprints", new[] { "IdProject" });
            DropIndex("dbo.Jobs", new[] { "TableStatus" });
            DropIndex("dbo.Jobs", new[] { "BacklogStatus" });
            DropIndex("dbo.Jobs", new[] { "IdUser" });
            DropIndex("dbo.Jobs", new[] { "IdSprint" });
            DropIndex("dbo.HistoryJobs", new[] { "IdUser" });
            DropIndex("dbo.HistoryJobs", new[] { "IdJob" });
            DropTable("dbo.Users");
            DropTable("dbo.Projects");
            DropTable("dbo.Sprints");
            DropTable("dbo.Jobs");
            DropTable("dbo.HistoryJobs");
        }
    }
}
