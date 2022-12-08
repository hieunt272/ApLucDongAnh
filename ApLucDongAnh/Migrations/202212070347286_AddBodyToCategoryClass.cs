namespace ApLucDongAnh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBodyToCategoryClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleCategories", "Body", c => c.String());
            AddColumn("dbo.ArticleCategories", "SourceUrl", c => c.String(maxLength: 300));
            AddColumn("dbo.ArticleCategories", "Page", c => c.Int(nullable: false));
            AddColumn("dbo.Articles", "UrlSource", c => c.String(maxLength: 500));
            AddColumn("dbo.Products", "UrlSource", c => c.String(maxLength: 500));
            AddColumn("dbo.ProductCategories", "Body", c => c.String());
            AddColumn("dbo.ProductCategories", "SourceUrl", c => c.String(maxLength: 300));
            AddColumn("dbo.ProductCategories", "Page", c => c.Int(nullable: false));
            AlterColumn("dbo.ArticleCategories", "Description", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ArticleCategories", "Description", c => c.String());
            DropColumn("dbo.ProductCategories", "Page");
            DropColumn("dbo.ProductCategories", "SourceUrl");
            DropColumn("dbo.ProductCategories", "Body");
            DropColumn("dbo.Products", "UrlSource");
            DropColumn("dbo.Articles", "UrlSource");
            DropColumn("dbo.ArticleCategories", "Page");
            DropColumn("dbo.ArticleCategories", "SourceUrl");
            DropColumn("dbo.ArticleCategories", "Body");
        }
    }
}
