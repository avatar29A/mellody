namespace Hqub.Mellody.Music.Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStationEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Playlists", "Station_Id", c => c.Guid());
            CreateIndex("dbo.Playlists", "Station_Id");
            AddForeignKey("dbo.Playlists", "Station_Id", "dbo.Stations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Playlists", "Station_Id", "dbo.Stations");
            DropIndex("dbo.Playlists", new[] { "Station_Id" });
            DropColumn("dbo.Playlists", "Station_Id");
            DropTable("dbo.Stations");
        }
    }
}
