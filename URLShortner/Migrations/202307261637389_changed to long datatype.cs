namespace URLShortner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedtolongdatatype : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.UrlMaps");
            AlterColumn("dbo.UrlMaps", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.UrlMaps", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.UrlMaps");
            AlterColumn("dbo.UrlMaps", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.UrlMaps", "Id");
        }
    }
}
