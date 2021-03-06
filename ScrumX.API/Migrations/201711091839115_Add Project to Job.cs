namespace ScrumX.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjecttoJob : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "IdProject", c => c.Int(nullable: false));
            CreateIndex("dbo.Jobs", "IdProject");
            AddForeignKey("dbo.Jobs", "IdProject", "dbo.Projects", "IdProject", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "IdProject", "dbo.Projects");
            DropIndex("dbo.Jobs", new[] { "IdProject" });
            DropColumn("dbo.Jobs", "IdProject");
        }
    }
}
