namespace Attendance_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifiedDateinPermission : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Permissions", "PermissionDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Permissions", "SendingDate", c => c.DateTime());
            AlterColumn("dbo.Permissions", "ApprovementDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Permissions", "ApprovementDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Permissions", "SendingDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Permissions", "PermissionDate", c => c.DateTime(nullable: false));
        }
    }
}
