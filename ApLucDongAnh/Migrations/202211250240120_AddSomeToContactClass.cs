namespace ApLucDongAnh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSomeToContactClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "Region", c => c.Int(nullable: false));
            AddColumn("dbo.Contacts", "Company", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "Company");
            DropColumn("dbo.Contacts", "Region");
        }
    }
}
