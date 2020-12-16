// using JetBrains.Annotations;
// using Microsoft.EntityFrameworkCore;
// using System;
// using Oracle.EntityFrameworkCore.Infrastructure;
// using Volo.Abp.EntityFrameworkCore.DependencyInjection;
//
// namespace Volo.Abp.EntityFrameworkCore
// {
//     public static class AbpDbContextConfigurationContextOracleExtensions
//     {
//         public static DbContextOptionsBuilder UseOracle(
//            [NotNull] this AbpDbContextConfigurationContext context,
//            [CanBeNull] Action<OracleDbContextOptionsBuilder> oracleOptionsAction = null)
//         {
//             TODO: UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
//             if (context.ExistingConnection != null)
//             {
//                 return context.DbContextOptions.UseOracle(context.ExistingConnection, oracleOptionsAction);
//             }
//             else
//             {
//                 return context.DbContextOptions.UseOracle(context.ConnectionString, oracleOptionsAction);
//             }
//         }
//     }
// }
