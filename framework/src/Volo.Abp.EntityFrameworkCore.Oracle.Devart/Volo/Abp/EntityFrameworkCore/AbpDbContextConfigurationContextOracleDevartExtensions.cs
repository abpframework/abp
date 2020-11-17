// using JetBrains.Annotations;
// using Microsoft.EntityFrameworkCore;
// using System;
// using Devart.Data.Oracle.Entity;
// using Volo.Abp.EntityFrameworkCore.DependencyInjection;
//
// namespace Volo.Abp.EntityFrameworkCore
// {
//     public static class AbpDbContextConfigurationContextOracleDevartExtensions
//     {
//         public static DbContextOptionsBuilder UseOracle(
//            [NotNull] this AbpDbContextConfigurationContext context,
//            [CanBeNull] Action<OracleDbContextOptionsBuilder> oracleOptionsAction = null,
//            bool useExistingConnectionIfAvailable = false)
//         {
//             TODO: UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
//             if (useExistingConnectionIfAvailable && context.ExistingConnection != null)
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
