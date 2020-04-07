namespace Attendance_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConvertToDate : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Attendances");
            AlterColumn("dbo.Attendances", "Date", c => c.DateTime(nullable: false, storeType: "date"));
            AddPrimaryKey("dbo.Attendances", new[] { "Date", "StudentId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Attendances");
            AlterColumn("dbo.Attendances", "Date", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddPrimaryKey("dbo.Attendances", new[] { "Date", "StudentId" });
        }
    }
}
