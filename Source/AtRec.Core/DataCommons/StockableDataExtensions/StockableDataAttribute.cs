using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtRec.Core.DataCommons.StockableDataExtensions
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class StockableDataAttribute : Attribute
    {
        public Guid Id
        {
            get;
            private set;
        }

        public StockableDataAttribute(string idStr)
        {
            this.Id = Guid.Parse(idStr);
        }
    }
}
