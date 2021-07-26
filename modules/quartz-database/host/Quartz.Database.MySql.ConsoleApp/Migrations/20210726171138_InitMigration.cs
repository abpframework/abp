using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuartzDatabaseDemo.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QRTZ_CALENDARS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CALENDAR_NAME = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CALENDAR = table.Column<byte[]>(type: "longblob", maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_CALENDARS", x => new { x.SCHED_NAME, x.CALENDAR_NAME });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QRTZ_FIRED_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ENTRY_ID = table.Column<string>(type: "varchar(140)", maxLength: 140, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    INSTANCE_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FIRED_TIME = table.Column<long>(type: "bigint", nullable: false),
                    SCHED_TIME = table.Column<long>(type: "bigint", nullable: false),
                    PRIORITY = table.Column<int>(type: "int", nullable: false),
                    STATE = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_GROUP = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IS_NONCONCURRENT = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    REQUESTS_RECOVERY = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_FIRED_TRIGGERS", x => new { x.SCHED_NAME, x.ENTRY_ID });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QRTZ_JOB_DETAILS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_NAME = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_GROUP = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DESCRIPTION = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_CLASS_NAME = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IS_DURABLE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IS_NONCONCURRENT = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IS_UPDATE_DATA = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    REQUESTS_RECOVERY = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    JOB_DATA = table.Column<byte[]>(type: "longblob", maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_JOB_DETAILS", x => new { x.SCHED_NAME, x.JOB_NAME, x.JOB_GROUP });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QRTZ_LOCKS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LOCK_NAME = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_LOCKS", x => new { x.SCHED_NAME, x.LOCK_NAME });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QRTZ_PAUSED_TRIGGER_GRPS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_PAUSED_TRIGGER_GRPS", x => new { x.SCHED_NAME, x.TRIGGER_GROUP });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QRTZ_SCHEDULER_STATE",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    INSTANCE_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LAST_CHECKIN_TIME = table.Column<long>(type: "bigint", nullable: false),
                    CHECKIN_INTERVAL = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_SCHEDULER_STATE", x => new { x.SCHED_NAME, x.INSTANCE_NAME });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QRTZ_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_NAME = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_GROUP = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DESCRIPTION = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NEXT_FIRE_TIME = table.Column<long>(type: "bigint", nullable: true),
                    PREV_FIRE_TIME = table.Column<long>(type: "bigint", nullable: true),
                    PRIORITY = table.Column<int>(type: "int", nullable: true),
                    TRIGGER_STATE = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_TYPE = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    START_TIME = table.Column<long>(type: "bigint", nullable: false),
                    END_TIME = table.Column<long>(type: "bigint", nullable: true),
                    CALENDAR_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MISFIRE_INSTR = table.Column<short>(type: "smallint", nullable: true),
                    JOB_DATA = table.Column<byte[]>(type: "longblob", maxLength: 2147483647, nullable: true)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QRTZ_BLOB_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BLOB_DATA = table.Column<byte[]>(type: "longblob", maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_BLOB_TRIGGERS", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        name: "FK_QRTZ_BLOB_TRIGGERS_QRTZ_TRIGGERS_SCHED_NAME_TRIGGER_NAME_TRI~",
                        columns: x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        principalTable: "QRTZ_TRIGGERS",
                        principalColumns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QRTZ_CRON_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CRON_EXPRESSION = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TIME_ZONE_ID = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_CRON_TRIGGERS", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        name: "FK_QRTZ_CRON_TRIGGERS_QRTZ_TRIGGERS_SCHED_NAME_TRIGGER_NAME_TRI~",
                        columns: x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        principalTable: "QRTZ_TRIGGERS",
                        principalColumns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QRTZ_SIMPLE_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    REPEAT_COUNT = table.Column<long>(type: "bigint", nullable: false),
                    REPEAT_INTERVAL = table.Column<long>(type: "bigint", nullable: false),
                    TIMES_TRIGGERED = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_SIMPLE_TRIGGERS", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        name: "FK_QRTZ_SIMPLE_TRIGGERS_QRTZ_TRIGGERS_SCHED_NAME_TRIGGER_NAME_T~",
                        columns: x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        principalTable: "QRTZ_TRIGGERS",
                        principalColumns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QRTZ_SIMPROP_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    STR_PROP_1 = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    STR_PROP_2 = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    STR_PROP_3 = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    INT_PROP_1 = table.Column<int>(type: "int", nullable: true),
                    INT_PROP_2 = table.Column<int>(type: "int", nullable: true),
                    LONG_PROP_1 = table.Column<long>(type: "bigint", nullable: true),
                    LONG_PROP_2 = table.Column<long>(type: "bigint", nullable: true),
                    DEC_PROP_1 = table.Column<decimal>(type: "decimal(13,4)", precision: 13, scale: 4, nullable: true),
                    DEC_PROP_2 = table.Column<decimal>(type: "decimal(13,4)", precision: 13, scale: 4, nullable: true),
                    BOOL_PROP_1 = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    BOOL_PROP_2 = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    TIME_ZONE_ID = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_SIMPROP_TRIGGERS", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        name: "FK_QRTZ_SIMPROP_TRIGGERS_QRTZ_TRIGGERS_SCHED_NAME_TRIGGER_NAME_~",
                        columns: x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        principalTable: "QRTZ_TRIGGERS",
                        principalColumns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_FT_INST_JOB_REQ_RCVRY",
                table: "QRTZ_FIRED_TRIGGERS",
                columns: new[] { "SCHED_NAME", "INSTANCE_NAME", "REQUESTS_RECOVERY" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_FT_J_G",
                table: "QRTZ_FIRED_TRIGGERS",
                columns: new[] { "SCHED_NAME", "JOB_NAME", "JOB_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_FT_JG",
                table: "QRTZ_FIRED_TRIGGERS",
                columns: new[] { "SCHED_NAME", "JOB_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_FT_T_G",
                table: "QRTZ_FIRED_TRIGGERS",
                columns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_FT_TG",
                table: "QRTZ_FIRED_TRIGGERS",
                columns: new[] { "SCHED_NAME", "TRIGGER_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_FT_TRIG_INST_NAME",
                table: "QRTZ_FIRED_TRIGGERS",
                columns: new[] { "SCHED_NAME", "INSTANCE_NAME" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_J_GRP",
                table: "QRTZ_JOB_DETAILS",
                columns: new[] { "SCHED_NAME", "JOB_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_J_REQ_RECOVERY",
                table: "QRTZ_JOB_DETAILS",
                columns: new[] { "SCHED_NAME", "REQUESTS_RECOVERY" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_C",
                table: "QRTZ_TRIGGERS",
                columns: new[] { "SCHED_NAME", "CALENDAR_NAME" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_G",
                table: "QRTZ_TRIGGERS",
                columns: new[] { "SCHED_NAME", "TRIGGER_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_J",
                table: "QRTZ_TRIGGERS",
                columns: new[] { "SCHED_NAME", "JOB_NAME", "JOB_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_JG",
                table: "QRTZ_TRIGGERS",
                columns: new[] { "SCHED_NAME", "JOB_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_N_G_STATE",
                table: "QRTZ_TRIGGERS",
                columns: new[] { "SCHED_NAME", "TRIGGER_GROUP", "TRIGGER_STATE" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_N_STATE",
                table: "QRTZ_TRIGGERS",
                columns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP", "TRIGGER_STATE" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_NEXT_FIRE_TIME",
                table: "QRTZ_TRIGGERS",
                columns: new[] { "SCHED_NAME", "NEXT_FIRE_TIME" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_NFT_MISFIRE",
                table: "QRTZ_TRIGGERS",
                columns: new[] { "SCHED_NAME", "MISFIRE_INSTR", "NEXT_FIRE_TIME" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_NFT_ST",
                table: "QRTZ_TRIGGERS",
                columns: new[] { "SCHED_NAME", "TRIGGER_STATE", "NEXT_FIRE_TIME" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_NFT_ST_MISFIRE",
                table: "QRTZ_TRIGGERS",
                columns: new[] { "SCHED_NAME", "MISFIRE_INSTR", "NEXT_FIRE_TIME", "TRIGGER_STATE" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_NFT_ST_MISFIRE_GRP",
                table: "QRTZ_TRIGGERS",
                columns: new[] { "SCHED_NAME", "MISFIRE_INSTR", "NEXT_FIRE_TIME", "TRIGGER_GROUP", "TRIGGER_STATE" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_STATE",
                table: "QRTZ_TRIGGERS",
                columns: new[] { "SCHED_NAME", "TRIGGER_STATE" });
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
                name: "QRTZ_SIMPROP_TRIGGERS");

            migrationBuilder.DropTable(
                name: "QRTZ_TRIGGERS");

            migrationBuilder.DropTable(
                name: "QRTZ_JOB_DETAILS");
        }
    }
}
