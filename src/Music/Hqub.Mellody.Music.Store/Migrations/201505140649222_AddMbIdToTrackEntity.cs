namespace Hqub.Mellody.Music.Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMbIdToTrackEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tracks", "MbId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tracks", "MbId");
        }
    }
}
