namespace Hqub.Mellody.Music.Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexToTrackEntity : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Tracks", "MbId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tracks", new[] { "MbId" });
        }
    }
}
