namespace Attendance_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableAndDateTime2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Attendances");
            AlterColumn("dbo.Attendances", "Date", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Attendances", "Arrival", c => c.DateTime());
            AlterColumn("dbo.Attendances", "Departure", c => c.DateTime());
            AddPrimaryKey("dbo.Attendances", new[] { "Date", "StudentId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Attendances");
            AlterColumn("dbo.Attendances", "Departure", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Attendances", "Arrival", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Attendances", "Date", c => c.DateTime(nullable: false));
            AddPrimaryKey("dbo.Attendances", new[] { "Date", "StudentId" });
        }
    }
}
