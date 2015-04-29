namespace Hqub.Mellody.Music.Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Playlists",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tracks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Artist = c.String(),
                        Duration = c.Int(nullable: false),
                        Quality = c.Int(nullable: false),
                        Playlist_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Playlists", t => t.Playlist_Id)
                .Index(t => t.Playlist_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tracks", "Playlist_Id", "dbo.Playlists");
            DropIndex("dbo.Tracks", new[] { "Playlist_Id" });
            DropTable("dbo.Tracks");
            DropTable("dbo.Playlists");
        }
    }
}
