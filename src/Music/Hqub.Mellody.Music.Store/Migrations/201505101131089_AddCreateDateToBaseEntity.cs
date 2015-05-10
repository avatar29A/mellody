namespace Hqub.Mellody.Music.Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreateDateToBaseEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Playlists", "HashDescription", c => c.String());
            AddColumn("dbo.Playlists", "CreateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Playlists", "UpdateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tracks", "CreateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tracks", "UpdateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Stations", "CreateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Stations", "UpdateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stations", "UpdateTime");
            DropColumn("dbo.Stations", "CreateTime");
            DropColumn("dbo.Tracks", "UpdateTime");
            DropColumn("dbo.Tracks", "CreateTime");
            DropColumn("dbo.Playlists", "UpdateTime");
            DropColumn("dbo.Playlists", "CreateTime");
            DropColumn("dbo.Playlists", "HashDescription");
        }
    }
}
