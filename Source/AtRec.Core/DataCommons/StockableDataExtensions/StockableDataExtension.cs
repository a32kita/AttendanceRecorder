using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AtRec.Core.DataCommons.StockableDataExtensions
{
    internal static class StockableDataExtension
    {
        public static Guid GetStockableDataTypeId(this Type stockableDataType)
        {
            if (!stockableDataType.IsValidStockableType())
                throw new InvalidOperationException();

            return stockableDataType.GetCustomAttribute<StockableDataAttribute>().Id;
        }

        public static bool IsValidStockableType(this Type stockableDataType)
        {
            //if (!stockableDataType.IsSubclassOf(typeof(IStockableData)))
            //    return false;

            if (!stockableDataType.GetInterfaces().Contains(typeof(IStockableData)))
                return false;

            var atts = stockableDataType.GetCustomAttributes<StockableDataAttribute>();
            return atts.Count() != 0;
        }
    }
}
