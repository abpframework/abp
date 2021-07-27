using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quartz.Database.Sqlite.ConsoleApp.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QRTZ_CALENDARS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    CALENDAR_NAME = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CALENDAR = table.Column<byte[]>(type: "BLOB", maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_CALENDARS", x => new { x.SCHED_NAME, x.CALENDAR_NAME });
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_FIRED_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    ENTRY_ID = table.Column<string>(type: "TEXT", maxLength: 140, nullable: false),
                    TRIGGER_NAME = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    TRIGGER_GROUP = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    INSTANCE_NAME = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    FIRED_TIME = table.Column<long>(type: "INTEGER", nullable: false),
                    SCHED_TIME = table.Column<long>(type: "INTEGER", nullable: false),
                    PRIORITY = table.Column<int>(type: "INTEGER", nullable: false),
                    STATE = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    JOB_NAME = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    JOB_GROUP = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    IS_NONCONCURRENT = table.Column<bool>(type: "INTEGER", nullable: true),
                    REQUESTS_RECOVERY = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_FIRED_TRIGGERS", x => new { x.SCHED_NAME, x.ENTRY_ID });
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_JOB_DETAILS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    JOB_NAME = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    JOB_GROUP = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    JOB_CLASS_NAME = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    IS_DURABLE = table.Column<bool>(type: "INTEGER", nullable: false),
                    IS_NONCONCURRENT = table.Column<bool>(type: "INTEGER", nullable: false),
                    IS_UPDATE_DATA = table.Column<bool>(type: "INTEGER", nullable: false),
                    REQUESTS_RECOVERY = table.Column<bool>(type: "INTEGER", nullable: false),
                    JOB_DATA = table.Column<byte[]>(type: "BLOB", maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_JOB_DETAILS", x => new { x.SCHED_NAME, x.JOB_NAME, x.JOB_GROUP });
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_LOCKS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    LOCK_NAME = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_LOCKS", x => new { x.SCHED_NAME, x.LOCK_NAME });
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_PAUSED_TRIGGER_GRPS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    TRIGGER_GROUP = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_PAUSED_TRIGGER_GRPS", x => new { x.SCHED_NAME, x.TRIGGER_GROUP });
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_SCHEDULER_STATE",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    INSTANCE_NAME = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    LAST_CHECKIN_TIME = table.Column<long>(type: "INTEGER", nullable: false),
                    CHECKIN_INTERVAL = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_SCHEDULER_STATE", x => new { x.SCHED_NAME, x.INSTANCE_NAME });
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    TRIGGER_NAME = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    TRIGGER_GROUP = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    JOB_NAME = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    JOB_GROUP = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    NEXT_FIRE_TIME = table.Column<long>(type: "INTEGER", nullable: true),
                    PREV_FIRE_TIME = table.Column<long>(type: "INTEGER", nullable: true),
                    PRIORITY = table.Column<int>(type: "INTEGER", nullable: true),
                    TRIGGER_STATE = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    TRIGGER_TYPE = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    START_TIME = table.Column<long>(type: "INTEGER", nullable: false),
                    END_TIME = table.Column<long>(type: "INTEGER", nullable: true),
                    CALENDAR_NAME = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    MISFIRE_INSTR = table.Column<short>(type: "INTEGER", nullable: true),
                    JOB_DATA = table.Column<byte[]>(type: "BLOB", maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_TRIGGERS", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        name: "FK_QRTZ_TRIGGERS_QRTZ_JOB_DETAILS_SCHED_NAME_JOB_NAME_JOB_GROUP",
                        columns: x => new { x.SCHED_NAME, x.JOB_NAME, x.JOB_GROUP },
                        principalTable: "QRTZ_JOB_DETAILS",
                        principalColumns: new[] { "SCHED_NAME", "JOB_NAME", "JOB_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_BLOB_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    TRIGGER_NAME = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    TRIGGER_GROUP = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    BLOB_DATA = table.Column<byte[]>(type: "BLOB", maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_BLOB_TRIGGERS", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        name: "FK_QRTZ_BLOB_TRIGGERS_QRTZ_TRIGGERS_SCHED_NAME_TRIGGER_NAME_TRIGGER_GROUP",
                        columns: x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        principalTable: "QRTZ_TRIGGERS",
                        principalColumns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_CRON_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    TRIGGER_NAME = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    TRIGGER_GROUP = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    CRON_EXPRESSION = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    TIME_ZONE_ID = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_CRON_TRIGGERS", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        name: "FK_QRTZ_CRON_TRIGGERS_QRTZ_TRIGGERS_SCHED_NAME_TRIGGER_NAME_TRIGGER_GROUP",
                        columns: x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        principalTable: "QRTZ_TRIGGERS",
                        principalColumns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_SIMPLE_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    TRIGGER_NAME = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    TRIGGER_GROUP = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    REPEAT_COUNT = table.Column<long>(type: "INTEGER", nullable: false),
                    REPEAT_INTERVAL = table.Column<long>(type: "INTEGER", nullable: false),
                    TIMES_TRIGGERED = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_SIMPLE_TRIGGERS", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        name: "FK_QRTZ_SIMPLE_TRIGGERS_QRTZ_TRIGGERS_SCHED_NAME_TRIGGER_NAME_TRIGGER_GROUP",
                        columns: x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        principalTable: "QRTZ_TRIGGERS",
                        principalColumns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_SIMPROP_TIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    TRIGGER_NAME = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    TRIGGER_GROUP = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    STR_PROP_1 = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    STR_PROP_2 = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    STR_PROP_3 = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    INT_PROP_1 = table.Column<int>(type: "INTEGER", nullable: true),
                    INT_PROP_2 = table.Column<int>(type: "INTEGER", nullable: true),
                    LONG_PROP_1 = table.Column<long>(type: "INTEGER", nullable: true),
                    LONG_PROP_2 = table.Column<long>(type: "INTEGER", nullable: true),
                    DEC_PROP_1 = table.Column<decimal>(type: "TEXT", precision: 13, scale: 4, nullable: true),
                    DEC_PROP_2 = table.Column<decimal>(type: "TEXT", precision: 13, scale: 4, nullable: true),
                    BOOL_PROP_1 = table.Column<bool>(type: "INTEGER", nullable: true),
                    BOOL_PROP_2 = table.Column<bool>(type: "INTEGER", nullable: true),
                    TIME_ZONE_ID = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_SIMPROP_TIGGERS", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        name: "FK_QRTZ_SIMPROP_TIGGERS_QRTZ_TRIGGERS_SCHED_NAME_TRIGGER_NAME_TRIGGER_GROUP",
                        columns: x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        principalTable: "QRTZ_TRIGGERS",
                        principalColumns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QRTZ_TRIGGERS_SCHED_NAME_JOB_NAME_JOB_GROUP",
                table: "QRTZ_TRIGGERS",
                columns: new[] { "SCHED_NAME", "JOB_NAME", "JOB_GROUP" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QRTZ_BLOB_TRIGGERS");

            migrationBuilder.DropTable(
                name: "QRTZ_CALENDARS");

            migrationBuilder.DropTable(
                name: "QRTZ_CRON_TRIGGERS");

            migrationBuilder.DropTable(
                name: "QRTZ_FIRED_TRIGGERS");

            migrationBuilder.DropTable(
                name: "QRTZ_LOCKS");

            migrationBuilder.DropTable(
                name: "QRTZ_PAUSED_TRIGGER_GRPS");

            migrationBuilder.DropTable(
                name: "QRTZ_SCHEDULER_STATE");

            migrationBuilder.DropTable(
                name: "QRTZ_SIMPLE_TRIGGERS");

            migrationBuilder.DropTable(
                name: "QRTZ_SIMPROP_TIGGERS");

            migrationBuilder.DropTable(
                name: "QRTZ_TRIGGERS");

            migrationBuilder.DropTable(
                name: "QRTZ_JOB_DETAILS");
        }
    }
}
