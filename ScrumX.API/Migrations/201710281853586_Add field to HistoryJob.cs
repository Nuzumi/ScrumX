namespace ScrumX.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddfieldtoHistoryJob : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HistoryJobs", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HistoryJobs", "Date");
        }
    }
}
