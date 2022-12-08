namespace ApLucDongAnh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrlSourceToProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "UrlSource", c => c.String(maxLength: 500));
            AlterColumn("dbo.Articles", "Image", c => c.String(maxLength: 500));
            AlterColumn("dbo.Projects", "StartDate", c => c.String(maxLength: 100));
            AlterColumn("dbo.Projects", "EndDate", c => c.String(maxLength: 100));
            DropColumn("dbo.ArticleCategories", "SourceUrl");
            DropColumn("dbo.ArticleCategories", "Page");
            DropColumn("dbo.ProductCategories", "SourceUrl");
            DropColumn("dbo.ProductCategories", "Page");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductCategories", "Page", c => c.Int(nullable: false));
            AddColumn("dbo.ProductCategories", "SourceUrl", c => c.String(maxLength: 300));
            AddColumn("dbo.ArticleCategories", "Page", c => c.Int(nullable: false));
            AddColumn("dbo.ArticleCategories", "SourceUrl", c => c.String(maxLength: 300));
            AlterColumn("dbo.Projects", "EndDate", c => c.String());
            AlterColumn("dbo.Projects", "StartDate", c => c.String());
            AlterColumn("dbo.Articles", "Image", c => c.String());
            DropColumn("dbo.Projects", "UrlSource");
        }
    }
}
