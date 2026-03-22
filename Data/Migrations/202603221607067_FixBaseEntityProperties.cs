namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixBaseEntityProperties : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "UpdatedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "UpdatedAt", c => c.DateTime(nullable: false));
        }
    }
}
