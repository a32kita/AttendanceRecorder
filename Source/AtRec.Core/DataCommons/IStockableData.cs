using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtRec.Core.DataCommons
{
    public interface IStockableData
    {
        string Name { get; }
        
        void WriteTo(Stream stream);

        void ReadFrom(Stream stream);

        object GetData();
    }
}
