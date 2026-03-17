namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update04 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        CoverImageUrl = c.String(),
                        AuthorId = c.String(maxLength: 128),
                        PublishDate = c.DateTime(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PostId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tags", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Posts", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.Tags", new[] { "PostId" });
            DropIndex("dbo.Posts", new[] { "AuthorId" });
            DropTable("dbo.Tags");
            DropTable("dbo.Posts");
        }
    }
}
