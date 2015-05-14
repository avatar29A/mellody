namespace Hqub.Mellody.Music.Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPositionFieldToTrackEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tracks", "Position", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tracks", "Position");
        }
    }
}
