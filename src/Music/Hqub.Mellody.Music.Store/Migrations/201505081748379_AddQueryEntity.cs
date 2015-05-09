namespace Hqub.Mellody.Music.Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQueryEntity : DbMigration
    {
        public override void Up()
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
            
            AddColumn("dbo.Playlists", "Name", c => c.String());
            AddColumn("dbo.Playlists", "Query_Id", c => c.Guid());
            CreateIndex("dbo.Playlists", "Query_Id");
            AddForeignKey("dbo.Playlists", "Query_Id", "dbo.Queries", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Playlists", "Query_Id", "dbo.Queries");
            DropIndex("dbo.Playlists", new[] { "Query_Id" });
            DropColumn("dbo.Playlists", "Query_Id");
            DropColumn("dbo.Playlists", "Name");
            DropTable("dbo.Queries");
        }
    }
}
