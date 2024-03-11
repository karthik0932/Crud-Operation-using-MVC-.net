namespace curdoperation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changevalues : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "ModifiedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "ModifiedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.Products", "ModifiedBy", c => c.String());
            AlterColumn("dbo.Products", "CreatedBy", c => c.String());
        }
    }
}
