using UnityEngine;
using System.Collections.Generic;

namespace com.spacepuppy.Statistics
{

    public static class ExtensionMethods
    {

        public static double? GetStat(this IStatisticsTokenLedger ledger, string stat, string token = null)
        {
            double? value = null;
            return ledger?.TryGetStat(new StatId(stat, token), out value) ?? false ? value : null;
        }
        public static void SetStat(this IStatisticsTokenLedger ledger, string stat, double amount, string token = null) => ledger?.SetStat(new StatId(stat, token), amount);
        public static void AdjustStat(this IStatisticsTokenLedger ledger, string stat, double amount, string token = null) => ledger?.AdjustStat(new StatId(stat, token), amount);

        public static double GetStatOrDefault(this IStatisticsTokenLedgerService ledger, string stat, string token = null)
        {
            double? value = null;
            return (ledger?.TryGetStat(new StatId(stat, token), out value) ?? false) ? value.GetValueOrDefault() : 0d;
        }
        public static double GetStatOrDefault(this IStatisticsTokenLedgerService ledger, StatId stat)
        {
            double? value = null;
            return (ledger?.TryGetStat(stat, out value) ?? false) ? value.GetValueOrDefault() : 0d;
        }

        public static bool GetStatAsBool(this IStatisticsTokenLedgerService ledger, string stat, string token = null)
        {
            double? value = null;
            return (ledger?.TryGetStat(new StatId(stat, token), out value) ?? false) && value != null && value.Value != 0d;
        }
        public static bool GetStatAsBool(this IStatisticsTokenLedgerService ledger, StatId stat)
        {
            double? value = null;
            return (ledger?.TryGetStat(stat, out value) ?? false) && value != null && value.Value != 0d;
        }

    }
}
