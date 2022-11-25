namespace ApLucDongAnh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRequiredCompanyInContactClass : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacts", "Company", c => c.String(nullable: false, maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contacts", "Company", c => c.String());
        }
    }
}
