namespace ApLucDongAnh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProductionImage : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Abouts", "ProductionImage", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Abouts", "ProductionImage", c => c.String(maxLength: 500));
        }
    }
}
