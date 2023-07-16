using UnityEngine;
using System.Collections.Generic;

namespace com.spacepuppy.Statistics
{
    public static class ExtensionMethods
    {

        public static double GetStatOrDefault(this IStatisticsTokenLedgerService ledger, string stat, string token = null) => ledger?.GetStat(stat, token) ?? 0d;

        public static bool GetStatAsBool(this IStatisticsTokenLedgerService ledger, string stat, string token = null) => ledger?.GetStat(stat, token) != 0f;

    }
}
