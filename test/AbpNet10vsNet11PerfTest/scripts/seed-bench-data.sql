-- Load-test data added on top of the DbMigrator seed (identical in both projects).

-- Saving the email settings stores these two global rows. ProviderKey is NULL and PostgreSQL's unique index
-- treats NULLs as distinct, so concurrent first saves could insert duplicates; storing them once avoids that race.
INSERT INTO "AbpSettings" ("Id", "Name", "Value", "ProviderName", "ProviderKey")
VALUES (md5('Abp.Mailing.Smtp.UserName')::uuid, 'Abp.Mailing.Smtp.UserName', '', 'G', NULL),
       (md5('Abp.Mailing.Smtp.Domain')::uuid,   'Abp.Mailing.Smtp.Domain',   '', 'G', NULL);

-- Adds 1000 load-test users (bench_vu_1 .. bench_vu_1000) as clones of the seeded admin user:
-- same password hash and the admin role, so each virtual user logs in with its own account.
-- Ids and stamps are derived from the user name, so both projects get identical rows.
INSERT INTO "AbpUsers"
SELECT (jsonb_populate_record(NULL::"AbpUsers", to_jsonb(a) || jsonb_build_object(
    'Id',                 md5('bench_vu_' || n)::uuid,
    'UserName',           'bench_vu_' || n,
    'NormalizedUserName', upper('bench_vu_' || n),
    'Name',               'Bench',
    'Surname',            'VU ' || n,
    'Email',              'bench_vu_' || n || '@bench.local',
    'NormalizedEmail',    upper('bench_vu_' || n || '@bench.local'),
    'SecurityStamp',      md5('security_' || n),
    'ConcurrencyStamp',   md5('concurrency_' || n)
))).*
FROM "AbpUsers" a, generate_series(1, 1000) AS n
WHERE a."NormalizedUserName" = 'ADMIN';

INSERT INTO "AbpUserRoles" ("UserId", "RoleId", "TenantId")
SELECT md5('bench_vu_' || n)::uuid, r."Id", NULL
FROM "AbpRoles" r, generate_series(1, 1000) AS n
WHERE r."NormalizedName" = 'ADMIN';

-- Populate planner statistics and visibility maps on all tables before snapshotting the template database.
VACUUM ANALYZE;
