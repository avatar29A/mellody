namespace Hqub.Mellody.Music.Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveQueryTableAndAddHashFieldToPlaylist : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Playlists", "Query_Id", "dbo.Queries");
            DropIndex("dbo.Playlists", new[] { "Query_Id" });
            AddColumn("dbo.Playlists", "Hash", c => c.String(maxLength: 400));
            CreateIndex("dbo.Playlists", "Hash");
            DropColumn("dbo.Playlists", "Query_Id");
            DropTable("dbo.Queries");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Queries",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Hash = c.String(),
                        Descriptions = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Playlists", "Query_Id", c => c.Guid());
            DropIndex("dbo.Playlists", new[] { "Hash" });
            DropColumn("dbo.Playlists", "Hash");
            CreateIndex("dbo.Playlists", "Query_Id");
            AddForeignKey("dbo.Playlists", "Query_Id", "dbo.Queries", "Id");
        }
    }
}
