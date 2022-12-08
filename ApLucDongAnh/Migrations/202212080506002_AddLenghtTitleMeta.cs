namespace ApLucDongAnh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLenghtTitleMeta : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Articles", "TitleMeta", c => c.String(maxLength: 200));
            AlterColumn("dbo.Products", "TitleMeta", c => c.String(maxLength: 200));
            AlterColumn("dbo.Projects", "TitleMeta", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "TitleMeta", c => c.String(maxLength: 100));
            AlterColumn("dbo.Products", "TitleMeta", c => c.String(maxLength: 100));
            AlterColumn("dbo.Articles", "TitleMeta", c => c.String(maxLength: 100));
        }
    }
}
