using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.Quartz.Database.EntityFrameworkCore
{
    public static class QuartzDatabaseDbContextModelCreatingExtensions
    {
        public static void ConfigureQuartzDatabase(
            this ModelBuilder builder,
            Action<QuartzDatabaseModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            //TODO: sqlite database need to create triggers, any way?

            var options = new QuartzDatabaseModelBuilderConfigurationOptions(
                QuartzDatabaseDbProperties.DbTablePrefix,
                QuartzDatabaseDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            // QRTZ_BLOB_TRIGGERS
            builder.Entity<QuartzBlobTrigger>(b =>
            {
                b.ToTable(builder.TableNameConvert($"{options.TablePrefix}blob_triggers"), options.Schema);

                b.HasKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup });

                b.Property(x => x.SchedulerName).HasColumnName(builder.ColumnNameConvert("sched_name")).IsRequired();
                b.Property(x => x.TriggerName).HasColumnName(builder.ColumnNameConvert("trigger_name")).IsRequired();
                b.Property(x => x.TriggerGroup).HasColumnName(builder.ColumnNameConvert("trigger_group")).IsRequired();
                b.Property(x => x.BlobData).HasColumnName(builder.ColumnNameConvert("blob_data"));

                if (builder.IsUsingPostgreSql())
                {
                    b.Property(x => x.SchedulerName).HasColumnType("text");
                    b.Property(x => x.TriggerName).HasColumnType("text");
                    b.Property(x => x.TriggerGroup).HasColumnType("text");
                    b.Property(x => x.BlobData).HasColumnType("bytea");
                }
                else
                {
                    b.Property(x => x.SchedulerName).HasMaxLength(120);
                    b.Property(x => x.TriggerName).HasMaxLength(150);
                    b.Property(x => x.TriggerGroup).HasMaxLength(150);
                    b.Property(x => x.BlobData).HasMaxLength(int.MaxValue);
                }

                b.HasOne<QuartzTrigger>()
                  .WithMany(x => x.BlobTriggers)
                  .HasForeignKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup })
                  .OnDelete(DeleteBehavior.Cascade);
            });

            // QRTZ_CALENDARS
            builder.Entity<QuartzCalendar>(b =>
            {
                b.ToTable(builder.TableNameConvert($"{options.TablePrefix}calendars"), options.Schema);

                b.HasKey(x => new { x.SchedulerName, x.CalendarName });

                b.Property(x => x.SchedulerName).HasColumnName(builder.ColumnNameConvert("sched_name")).IsRequired();
                b.Property(x => x.CalendarName).HasColumnName(builder.ColumnNameConvert("calendar_name")).IsRequired();
                b.Property(x => x.Calendar).HasColumnName(builder.ColumnNameConvert("calendar")).IsRequired();

                if (builder.IsUsingPostgreSql())
                {
                    b.Property(x => x.SchedulerName).HasColumnType("text");
                    b.Property(x => x.CalendarName).HasColumnType("text");
                    b.Property(x => x.Calendar).HasColumnType("bytea");
                }
                else
                {
                    b.Property(x => x.SchedulerName).HasMaxLength(120);
                    b.Property(x => x.CalendarName).HasMaxLength(100);
                    b.Property(x => x.Calendar).HasMaxLength(int.MaxValue);
                }
            });

            // QRTZ_CRON_TRIGGERS
            builder.Entity<QuartzCronTrigger>(b =>
            {
                b.ToTable(builder.TableNameConvert($"{options.TablePrefix}cron_triggers"), options.Schema);

                b.HasKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup });

                b.Property(x => x.SchedulerName).HasColumnName(builder.ColumnNameConvert("sched_name")).IsRequired();
                b.Property(x => x.TriggerName).HasColumnName(builder.ColumnNameConvert("trigger_name")).IsRequired();
                b.Property(x => x.TriggerGroup).HasColumnName(builder.ColumnNameConvert("trigger_group")).IsRequired();
                b.Property(x => x.CronExpression).HasColumnName(builder.ColumnNameConvert("cron_expression")).IsRequired();
                b.Property(x => x.TimeZoneId).HasColumnName(builder.ColumnNameConvert("time_zone_id"));

                if (builder.IsUsingPostgreSql())
                {
                    b.Property(x => x.SchedulerName).HasColumnType("text");
                    b.Property(x => x.TriggerName).HasColumnType("text");
                    b.Property(x => x.TriggerGroup).HasColumnType("text");
                    b.Property(x => x.CronExpression).HasColumnType("text");
                    b.Property(x => x.TimeZoneId).HasColumnType("text");
                }
                else
                {
                    b.Property(x => x.SchedulerName).HasMaxLength(120);
                    b.Property(x => x.TriggerName).HasMaxLength(150);
                    b.Property(x => x.TriggerGroup).HasMaxLength(150);
                    b.Property(x => x.CronExpression).HasMaxLength(250);
                    b.Property(x => x.TimeZoneId).HasMaxLength(80);
                }

                b.HasOne<QuartzTrigger>()
                  .WithMany(x => x.CronTriggers)
                  .HasForeignKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup })
                  .OnDelete(DeleteBehavior.Cascade);
            });

            // QRTZ_FIRED_TRIGGERS
            builder.Entity<QuartzFiredTrigger>(b =>
            {
                b.ToTable(builder.TableNameConvert($"{options.TablePrefix}fired_triggers"), options.Schema);

                b.HasKey(x => new { x.SchedulerName, x.EntryId });

                b.Property(x => x.SchedulerName).HasColumnName(builder.ColumnNameConvert("sched_name")).IsRequired();
                b.Property(x => x.EntryId).HasColumnName(builder.ColumnNameConvert("entry_id")).IsRequired();
                b.Property(x => x.TriggerName).HasColumnName(builder.ColumnNameConvert("trigger_name")).IsRequired();
                b.Property(x => x.TriggerGroup).HasColumnName(builder.ColumnNameConvert("trigger_group")).IsRequired();
                b.Property(x => x.InstanceName).HasColumnName(builder.ColumnNameConvert("instance_name")).IsRequired();
                b.Property(x => x.FiredTime).HasColumnName(builder.ColumnNameConvert("fired_time")).IsRequired();
                b.Property(x => x.ScheduledTime).HasColumnName(builder.ColumnNameConvert("sched_time")).IsRequired();
                b.Property(x => x.Priority).HasColumnName(builder.ColumnNameConvert("priority")).IsRequired();
                b.Property(x => x.State).HasColumnName(builder.ColumnNameConvert("state")).IsRequired();
                b.Property(x => x.JobName).HasColumnName(builder.ColumnNameConvert("job_name"));
                b.Property(x => x.JobGroup).HasColumnName(builder.ColumnNameConvert("job_group"));
                b.Property(x => x.IsNonConcurrent).HasColumnName(builder.ColumnNameConvert("is_nonconcurrent"));
                b.Property(x => x.RequestsRecovery).HasColumnName(builder.ColumnNameConvert("requests_recovery"));

                if (builder.IsUsingPostgreSql())
                {
                    b.Property(x => x.SchedulerName).HasColumnType("text");
                    b.Property(x => x.EntryId).HasColumnType("text");
                    b.Property(x => x.TriggerName).HasColumnType("text");
                    b.Property(x => x.TriggerGroup).HasColumnType("text");
                    b.Property(x => x.InstanceName).HasColumnType("text");
                    b.Property(x => x.State).HasColumnType("text");
                    b.Property(x => x.JobName).HasColumnType("text");
                    b.Property(x => x.JobGroup).HasColumnType("text");
                    b.Property(x => x.IsNonConcurrent).IsRequired();
                }
                else
                {
                    b.Property(x => x.SchedulerName).HasMaxLength(120);
                    b.Property(x => x.EntryId).HasMaxLength(140);
                    b.Property(x => x.TriggerName).HasMaxLength(200);
                    b.Property(x => x.TriggerGroup).HasMaxLength(200);
                    b.Property(x => x.InstanceName).HasMaxLength(200);
                    b.Property(x => x.State).HasMaxLength(16);
                    if (builder.IsUsingFirebird() || builder.IsUsingSqlServer() || builder.IsUsingSqlite())
                    {
                        b.Property(x => x.JobName).HasMaxLength(150);
                        b.Property(x => x.JobGroup).HasMaxLength(150);
                    }
                    else
                    {
                        b.Property(x => x.JobName).HasMaxLength(200);
                        b.Property(x => x.JobGroup).HasMaxLength(200);
                    }
                    b.Property(x => x.IsNonConcurrent);
                    b.Property(x => x.RequestsRecovery);
                }

                if (builder.IsUsingSqlServer())
                {
                    b.HasIndex(x => new { x.SchedulerName, x.RequestsRecovery })
                      .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}ft_inst_job_req_rcvry"));
                    b.HasIndex(x => new { x.SchedulerName, x.JobGroup, x.JobName })
                      .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}ft_g_j"));
                    b.HasIndex(x => new { x.SchedulerName, x.TriggerGroup, x.TriggerName })
                      .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}ft_g_t"));
                }
                else if (builder.IsUsingPostgreSql())
                {
                    b.HasIndex(x => x.TriggerName)
                      .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}ft_trig_name"));
                    b.HasIndex(x => x.TriggerGroup)
                      .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}ft_trig_group"));
                    b.HasIndex(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup })
                      .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}ft_trig_nm_gp"));
                    b.HasIndex(x => x.InstanceName)
                      .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}ft_trig_inst_name"));
                    b.HasIndex(x => x.JobName)
                      .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}ft_job_name"));
                    b.HasIndex(x => x.JobGroup)
                      .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}ft_job_group"));
                    b.HasIndex(x => x.RequestsRecovery)
                      .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}ft_job_req_recovery"));
                }
                else if (builder.IsUsingOracle() || builder.IsUsingMySQL() || builder.IsUsingFirebird())
                {
                    b.HasIndex(x => new { x.SchedulerName, x.InstanceName })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}ft_trig_inst_name"));
                    b.HasIndex(x => new { x.SchedulerName, x.InstanceName, x.RequestsRecovery })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}ft_inst_job_req_rcvry"));
                    b.HasIndex(x => new { x.SchedulerName, x.JobName, x.JobGroup })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}ft_j_g"));
                    b.HasIndex(x => new { x.SchedulerName, x.JobGroup })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}ft_jg"));
                    b.HasIndex(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}ft_t_g"));
                    b.HasIndex(x => new { x.SchedulerName, x.TriggerGroup })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}ft_tg"));
                }
            });

            // QRTZ_JOB_DETAILS
            builder.Entity<QuartzJobDetail>(b =>
            {
                b.ToTable(builder.TableNameConvert($"{options.TablePrefix}job_details"), options.Schema);

                b.HasKey(x => new { x.SchedulerName, x.JobName, x.JobGroup });

                b.Property(x => x.SchedulerName).HasColumnName(builder.ColumnNameConvert("sched_name")).IsRequired();
                b.Property(x => x.JobName).HasColumnName(builder.ColumnNameConvert("job_name")).IsRequired();
                b.Property(x => x.JobGroup).HasColumnName(builder.ColumnNameConvert("job_group")).IsRequired();
                b.Property(x => x.Description).HasColumnName(builder.ColumnNameConvert("description"));
                b.Property(x => x.JobClassName).HasColumnName(builder.ColumnNameConvert("job_class_name")).IsRequired();
                b.Property(x => x.IsDurable).HasColumnName(builder.ColumnNameConvert("is_durable")).IsRequired();
                b.Property(x => x.IsNonConcurrent).HasColumnName(builder.ColumnNameConvert("is_nonconcurrent")).IsRequired();
                b.Property(x => x.IsUpdateData).HasColumnName(builder.ColumnNameConvert("is_update_data")).IsRequired();
                b.Property(x => x.RequestsRecovery).HasColumnName(builder.ColumnNameConvert("requests_recovery")).IsRequired();
                b.Property(x => x.JobData).HasColumnName(builder.ColumnNameConvert("job_data"));

                if (builder.IsUsingPostgreSql())
                {
                    b.Property(x => x.SchedulerName).HasColumnType("text");
                    b.Property(x => x.JobName).HasColumnType("text");
                    b.Property(x => x.JobGroup).HasColumnType("text");
                    b.Property(x => x.Description).HasColumnType("text");
                    b.Property(x => x.JobClassName).HasColumnType("text");
                    b.Property(x => x.JobData).HasColumnType("bytea");
                }
                else
                {
                    b.Property(x => x.SchedulerName).HasMaxLength(120);
                    b.Property(x => x.JobName).HasMaxLength(150);
                    b.Property(x => x.JobGroup).HasMaxLength(150);
                    b.Property(x => x.Description).HasMaxLength(250);
                    b.Property(x => x.JobClassName).HasMaxLength(250);
                    b.Property(x => x.JobData).HasMaxLength(int.MaxValue);
                }

                if (builder.IsUsingPostgreSql())
                {
                    b.HasIndex(x => x.RequestsRecovery)
                       .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}j_req_recovery"));
                }
                else if (builder.IsUsingOracle() || builder.IsUsingMySQL() || builder.IsUsingFirebird())
                {
                    b.HasIndex(x => new { x.SchedulerName, x.RequestsRecovery })
                       .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}j_req_recovery"));
                    b.HasIndex(x => new { x.SchedulerName, x.JobGroup })
                       .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}j_grp"));
                }
            });

            // QRTZ_LOCKS
            builder.Entity<QuartzLock>(b =>
            {
                b.ToTable(builder.TableNameConvert($"{options.TablePrefix}locks"), options.Schema);

                b.HasKey(x => new { x.SchedulerName, x.LockName });

                b.Property(x => x.SchedulerName).HasColumnName(builder.ColumnNameConvert("sched_name")).IsRequired();
                b.Property(x => x.LockName).HasColumnName(builder.ColumnNameConvert("lock_name")).IsRequired();

                if (builder.IsUsingPostgreSql())
                {
                    b.Property(x => x.SchedulerName).HasColumnType("text");
                    b.Property(x => x.LockName).HasColumnType("text");
                }
                else
                {
                    b.Property(x => x.SchedulerName).HasMaxLength(120);
                    b.Property(x => x.LockName).HasMaxLength(40);
                }
            });

            // QRTZ_PAUSED_TRIGGER_GRPS
            builder.Entity<QuartzPausedTriggerGroup>(b =>
            {
                b.ToTable(builder.TableNameConvert($"{options.TablePrefix}paused_trigger_grps"), options.Schema);

                b.HasKey(x => new { x.SchedulerName, x.TriggerGroup });

                b.Property(x => x.SchedulerName).HasColumnName(builder.ColumnNameConvert("sched_name")).IsRequired();
                b.Property(x => x.TriggerGroup).HasColumnName(builder.ColumnNameConvert("trigger_group")).IsRequired();

                if (builder.IsUsingPostgreSql())
                {
                    b.Property(x => x.SchedulerName).HasColumnType("text");
                    b.Property(x => x.TriggerGroup).HasColumnType("text");
                }
                else
                {
                    b.Property(x => x.SchedulerName).HasMaxLength(120);
                    b.Property(x => x.TriggerGroup).HasMaxLength(150);
                }
            });

            // QRTZ_SCHEDULER_STATE
            builder.Entity<QuartzSchedulerState>(b =>
            {
                b.ToTable(builder.TableNameConvert($"{options.TablePrefix}scheduler_state"), options.Schema);

                b.HasKey(x => new { x.SchedulerName, x.InstanceName });

                b.Property(x => x.SchedulerName).HasColumnName(builder.ColumnNameConvert("sched_name")).IsRequired();
                b.Property(x => x.InstanceName).HasColumnName(builder.ColumnNameConvert("instance_name")).IsRequired();
                b.Property(x => x.LastCheckInTime).HasColumnName(builder.ColumnNameConvert("last_checkin_time")).IsRequired();
                b.Property(x => x.CheckInInterval).HasColumnName(builder.ColumnNameConvert("checkin_interval")).IsRequired();

                if (builder.IsUsingPostgreSql())
                {
                    b.Property(x => x.SchedulerName).HasColumnType("text");
                    b.Property(x => x.InstanceName).HasColumnType("text");
                }
                else
                {
                    b.Property(x => x.SchedulerName).HasMaxLength(120);
                    b.Property(x => x.InstanceName).HasMaxLength(200);
                }
            });

            // QRTZ_SIMPROP_TRIGGERS
            builder.Entity<QuartzSimplePropertyTrigger>(b =>
            {
                b.ToTable(builder.TableNameConvert($"{options.TablePrefix}simprop_tiggers"), options.Schema);

                b.HasKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup });

                b.Property(x => x.SchedulerName).HasColumnName(builder.ColumnNameConvert("sched_name")).IsRequired();
                b.Property(x => x.TriggerName).HasColumnName(builder.ColumnNameConvert("trigger_name")).IsRequired();
                b.Property(x => x.TriggerGroup).HasColumnName(builder.ColumnNameConvert("trigger_group")).IsRequired();
                b.Property(x => x.StringProperty1).HasColumnName(builder.ColumnNameConvert("str_prop_1"));
                b.Property(x => x.StringProperty2).HasColumnName(builder.ColumnNameConvert("str_prop_2"));
                b.Property(x => x.StringProperty3).HasColumnName(builder.ColumnNameConvert("str_prop_3"));
                b.Property(x => x.IntegerProperty1).HasColumnName(builder.ColumnNameConvert("int_prop_1"));
                b.Property(x => x.IntegerProperty2).HasColumnName(builder.ColumnNameConvert("int_prop_2"));
                b.Property(x => x.LongProperty1).HasColumnName(builder.ColumnNameConvert("long_prop_1"));
                b.Property(x => x.LongProperty2).HasColumnName(builder.ColumnNameConvert("long_prop_2"));
                b.Property(x => x.DecimalProperty1).HasColumnName(builder.ColumnNameConvert("dec_prop_1"));
                b.Property(x => x.DecimalProperty2).HasColumnName(builder.ColumnNameConvert("dec_prop_2"));
                b.Property(x => x.BooleanProperty1).HasColumnName(builder.ColumnNameConvert("bool_prop_1"));
                b.Property(x => x.BooleanProperty2).HasColumnName(builder.ColumnNameConvert("bool_prop_2"));
                b.Property(x => x.TimeZoneId).HasColumnName(builder.ColumnNameConvert("time_zone_id"));

                if (builder.IsUsingPostgreSql())
                {
                    b.Property(x => x.SchedulerName).HasColumnType("text");
                    b.Property(x => x.TriggerName).HasColumnType("text");
                    b.Property(x => x.TriggerGroup).HasColumnType("text");
                    b.Property(x => x.StringProperty1).HasColumnType("text");
                    b.Property(x => x.StringProperty2).HasColumnType("text");
                    b.Property(x => x.StringProperty3).HasColumnType("text");
                    b.Property(x => x.DecimalProperty1).HasColumnType("numeric");
                    b.Property(x => x.DecimalProperty2).HasColumnType("numeric");
                    b.Property(x => x.TimeZoneId).HasColumnType("text");
                }
                else
                {
                    b.Property(x => x.SchedulerName).HasMaxLength(120);
                    b.Property(x => x.TriggerName).HasMaxLength(150);
                    b.Property(x => x.TriggerGroup).HasMaxLength(150);
                    b.Property(x => x.StringProperty1).HasMaxLength(512);
                    b.Property(x => x.StringProperty2).HasMaxLength(512);
                    b.Property(x => x.StringProperty3).HasMaxLength(512);
                    b.Property(x => x.DecimalProperty1).HasPrecision(13, 4);
                    b.Property(x => x.DecimalProperty2).HasPrecision(13, 4);
                    b.Property(x => x.TimeZoneId).HasMaxLength(80);
                }

                b.HasOne<QuartzTrigger>()
                  .WithMany(x => x.SimplePropertyTriggers)
                  .HasForeignKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup })
                  .OnDelete(DeleteBehavior.Cascade);
            });

            // QRTZ_SCHEDULER_STATE
            builder.Entity<QuartzSimpleTrigger>(b =>
            {
                b.ToTable(builder.TableNameConvert($"{options.TablePrefix}simple_triggers"), options.Schema);

                b.HasKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup });

                b.Property(x => x.SchedulerName).HasColumnName(builder.ColumnNameConvert("sched_name")).IsRequired();
                b.Property(x => x.TriggerName).HasColumnName(builder.ColumnNameConvert("trigger_name")).IsRequired();
                b.Property(x => x.TriggerGroup).HasColumnName(builder.ColumnNameConvert("trigger_group")).IsRequired();
                b.Property(x => x.RepeatCount).HasColumnName(builder.ColumnNameConvert("repeat_count")).IsRequired();
                b.Property(x => x.RepeatInterval).HasColumnName(builder.ColumnNameConvert("repeat_interval")).IsRequired();
                b.Property(x => x.TimesTriggered).HasColumnName(builder.ColumnNameConvert("times_triggered")).IsRequired();

                if (builder.IsUsingPostgreSql())
                {
                    b.Property(x => x.SchedulerName).HasColumnType("text");
                    b.Property(x => x.TriggerName).HasColumnType("text");
                    b.Property(x => x.TriggerGroup).HasColumnType("text");
                }
                else
                {
                    b.Property(x => x.SchedulerName).HasMaxLength(120);
                    b.Property(x => x.TriggerName).HasMaxLength(150);
                    b.Property(x => x.TriggerGroup).HasMaxLength(150);
                }

                b.HasOne<QuartzTrigger>()
                  .WithMany(x => x.SimpleTriggers)
                  .HasForeignKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup })
                  .OnDelete(DeleteBehavior.Cascade);
            });

            // QRTZ_TRIGGERS
            builder.Entity<QuartzTrigger>(b =>
            {
                b.ToTable(builder.TableNameConvert($"{options.TablePrefix}triggers"), options.Schema);

                b.HasKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup });

                b.Property(x => x.SchedulerName).HasColumnName(builder.ColumnNameConvert("sched_name")).IsRequired();
                b.Property(x => x.TriggerName).HasColumnName(builder.ColumnNameConvert("trigger_name")).IsRequired();
                b.Property(x => x.TriggerGroup).HasColumnName(builder.ColumnNameConvert("trigger_group")).IsRequired();
                b.Property(x => x.JobName).HasColumnName(builder.ColumnNameConvert("job_name")).IsRequired();
                b.Property(x => x.JobGroup).HasColumnName(builder.ColumnNameConvert("job_group")).IsRequired();
                b.Property(x => x.Description).HasColumnName(builder.ColumnNameConvert("description"));
                b.Property(x => x.NextFireTime).HasColumnName(builder.ColumnNameConvert("next_fire_time"));
                b.Property(x => x.PreviousFireTime).HasColumnName(builder.ColumnNameConvert("prev_fire_time"));
                b.Property(x => x.Priority).HasColumnName(builder.ColumnNameConvert("priority"));
                b.Property(x => x.TriggerState).HasColumnName(builder.ColumnNameConvert("trigger_state")).IsRequired();
                b.Property(x => x.TriggerType).HasColumnName(builder.ColumnNameConvert("trigger_type")).IsRequired();
                b.Property(x => x.StartTime).HasColumnName(builder.ColumnNameConvert("start_time")).IsRequired();
                b.Property(x => x.EndTime).HasColumnName(builder.ColumnNameConvert("end_time"));
                b.Property(x => x.CalendarName).HasColumnName(builder.ColumnNameConvert("calendar_name"));
                b.Property(x => x.MisfireInstruction).HasColumnName(builder.ColumnNameConvert("misfire_instr"));
                b.Property(x => x.JobData).HasColumnName(builder.ColumnNameConvert("job_data"));

                if (builder.IsUsingPostgreSql())
                {
                    b.Property(x => x.SchedulerName).HasColumnType("text");
                    b.Property(x => x.TriggerName).HasColumnType("text");
                    b.Property(x => x.TriggerGroup).HasColumnType("text");
                    b.Property(x => x.JobName).HasColumnType("text");
                    b.Property(x => x.JobGroup).HasColumnType("text");
                    b.Property(x => x.Description).HasColumnType("text");
                    b.Property(x => x.TriggerState).HasColumnType("text");
                    b.Property(x => x.TriggerType).HasColumnType("text");
                    b.Property(x => x.CalendarName).HasColumnType("text");
                    b.Property(x => x.JobData).HasColumnType("bytea");
                }
                else
                {
                    b.Property(x => x.SchedulerName).HasMaxLength(120);
                    b.Property(x => x.TriggerName).HasMaxLength(150);
                    b.Property(x => x.TriggerGroup).HasMaxLength(150);
                    b.Property(x => x.JobName).HasMaxLength(150);
                    b.Property(x => x.JobGroup).HasMaxLength(150);
                    b.Property(x => x.Description).HasMaxLength(250);
                    b.Property(x => x.TriggerState).HasMaxLength(16);
                    b.Property(x => x.TriggerType).HasMaxLength(8);
                    b.Property(x => x.CalendarName).HasMaxLength(200);
                    b.Property(x => x.JobData).HasMaxLength(int.MaxValue);
                }

                b.HasOne<QuartzJobDetail>()
                  .WithMany(x => x.Triggers)
                  .HasForeignKey(x => new { x.SchedulerName, x.JobName, x.JobGroup })
                  .IsRequired();

                if (builder.IsUsingSqlServer())
                {
                    b.HasIndex(x => new { x.SchedulerName, x.JobGroup, x.JobName })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_g_j"));
                    b.HasIndex(x => new { x.SchedulerName, x.CalendarName })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_c"));

                    b.HasIndex(x => new { x.SchedulerName, x.TriggerGroup, x.TriggerState })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_n_g_state"));
                    b.HasIndex(x => new { x.SchedulerName, x.TriggerState })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_state"));
                    b.HasIndex(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup, x.TriggerState })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_n_state"));
                    b.HasIndex(x => new { x.SchedulerName, x.NextFireTime })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_next_fire_time"));
                    b.HasIndex(x => new { x.SchedulerName, x.TriggerState, x.NextFireTime })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_nft_st"));
                    b.HasIndex(x => new { x.SchedulerName, x.MisfireInstruction, x.NextFireTime, x.TriggerState })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_nft_st_misfire"));
                    b.HasIndex(x => new { x.SchedulerName, x.MisfireInstruction, x.NextFireTime, x.TriggerGroup, x.TriggerState })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_nft_st_misfire_grp"));
                }
                else if (builder.IsUsingPostgreSql())
                {

                    b.HasIndex(x => x.NextFireTime)
                      .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_next_fire_time"));
                    b.HasIndex(x => x.TriggerState)
                      .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_state"));
                    b.HasIndex(x => new { x.NextFireTime, x.TriggerState })
                      .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_nft_st"));
                }
                else if (builder.IsUsingOracle() || builder.IsUsingMySQL() || builder.IsUsingFirebird())
                {
                    b.HasIndex(x => new { x.SchedulerName, x.JobName, x.JobGroup })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_j"));
                    b.HasIndex(x => new { x.SchedulerName, x.JobGroup })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_jg"));
                    b.HasIndex(x => new { x.SchedulerName, x.CalendarName })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_c"));
                    b.HasIndex(x => new { x.SchedulerName, x.TriggerGroup })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_g"));
                    b.HasIndex(x => new { x.SchedulerName, x.TriggerState })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_state"));
                    b.HasIndex(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup, x.TriggerState })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_n_state"));
                    b.HasIndex(x => new { x.SchedulerName, x.TriggerGroup, x.TriggerState })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_n_g_state"));
                    b.HasIndex(x => new { x.SchedulerName, x.NextFireTime })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_next_fire_time"));
                    b.HasIndex(x => new { x.SchedulerName, x.TriggerState, x.NextFireTime })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_nft_st"));
                    b.HasIndex(x => new { x.SchedulerName, x.MisfireInstruction, x.NextFireTime })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_nft_misfire"));
                    b.HasIndex(x => new { x.SchedulerName, x.MisfireInstruction, x.NextFireTime, x.TriggerState })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_nft_st_misfire"));
                    b.HasIndex(x => new { x.SchedulerName, x.MisfireInstruction, x.NextFireTime, x.TriggerGroup, x.TriggerState })
                        .HasDatabaseName(builder.IndexNameConvert($"idx_{options.TablePrefix}t_nft_st_misfire_grp"));
                }
            });

            builder.TryConfigureObjectExtensions<QuartzDatabaseDbContext>();
        }

        private static string ColumnNameConvert(this ModelBuilder builder, string name)
        {
            if (builder.IsUsingPostgreSql())
            {
                return name.ToLowerInvariant();
            }

            return name.ToUpperInvariant();
        }

        private static string TableNameConvert(this ModelBuilder builder, string name)
        {
            if (builder.IsUsingPostgreSql() || builder.IsUsingOracle())
            {
                return name.ToLowerInvariant();
            }

            return name.ToUpperInvariant();
        }

        private static string IndexNameConvert(this ModelBuilder builder, string name)
        {
            if (builder.IsUsingPostgreSql() || builder.IsUsingOracle())
            {
                return name.ToLowerInvariant();
            }

            return name.ToUpperInvariant();
        }
    }
}
