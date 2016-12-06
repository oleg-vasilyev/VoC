namespace VoC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VoCDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Code = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WordTranslations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WordId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Probability = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Words", t => t.WordId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.WordId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.Words",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WordValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserHistory",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        Username = c.String(),
                        RequestCounter = c.Int(nullable: false),
                        LastRequest = c.DateTime(nullable: false),
                        AverageTime = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WordTranslations", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.WordTranslations", "WordId", "dbo.Words");
            DropIndex("dbo.WordTranslations", new[] { "LanguageId" });
            DropIndex("dbo.WordTranslations", new[] { "WordId" });
            DropTable("dbo.UserHistory");
            DropTable("dbo.Words");
            DropTable("dbo.WordTranslations");
            DropTable("dbo.Languages");
        }
    }
}
