namespace Hqub.Mellody.Music.Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddManyToManyBetweenPlaylistAndStation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Playlists", "Station_Id", "dbo.Stations");
            DropIndex("dbo.Playlists", new[] { "Station_Id" });
            CreateTable(
                "dbo.StationPlaylists",
                c => new
                    {
                        Station_Id = c.Guid(nullable: false),
                        Playlist_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Station_Id, t.Playlist_Id })
                .ForeignKey("dbo.Stations", t => t.Station_Id, cascadeDelete: true)
                .ForeignKey("dbo.Playlists", t => t.Playlist_Id, cascadeDelete: true)
                .Index(t => t.Station_Id)
                .Index(t => t.Playlist_Id);
            
            DropColumn("dbo.Playlists", "Station_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Playlists", "Station_Id", c => c.Guid());
            DropForeignKey("dbo.StationPlaylists", "Playlist_Id", "dbo.Playlists");
            DropForeignKey("dbo.StationPlaylists", "Station_Id", "dbo.Stations");
            DropIndex("dbo.StationPlaylists", new[] { "Playlist_Id" });
            DropIndex("dbo.StationPlaylists", new[] { "Station_Id" });
            DropTable("dbo.StationPlaylists");
            CreateIndex("dbo.Playlists", "Station_Id");
            AddForeignKey("dbo.Playlists", "Station_Id", "dbo.Stations", "Id");
        }
    }
}
