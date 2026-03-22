namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectsModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "UpdatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "UpdatedAt");
        }
    }
}
