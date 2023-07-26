namespace URLShortner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UrlMaps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LongUrl = c.String(nullable: false),
                        ShortUrlKey = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UrlMaps");
        }
    }
}
