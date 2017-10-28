namespace ScrumX.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsersProjects",
                c => new
                    {
                        IdUsersProject = c.Int(nullable: false, identity: true),
                        IdUser = c.Int(nullable: false),
                        IdProject = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdUsersProject)
                .ForeignKey("dbo.Projects", t => t.IdProject, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdUser)
                .Index(t => t.IdProject);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersProjects", "IdUser", "dbo.Users");
            DropForeignKey("dbo.UsersProjects", "IdProject", "dbo.Projects");
            DropIndex("dbo.UsersProjects", new[] { "IdProject" });
            DropIndex("dbo.UsersProjects", new[] { "IdUser" });
            DropTable("dbo.UsersProjects");
        }
    }
}
