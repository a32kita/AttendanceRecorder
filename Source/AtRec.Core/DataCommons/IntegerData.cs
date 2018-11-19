using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtRec.Core.DataCommons.StockableDataExtensions;

namespace AtRec.Core.DataCommons
{
    [StockableData("{4979110A-7F35-4246-9529-4945EE7EB8CD}")]
    public class IntegerData : StockData<int>
    {
        public override void ReadFrom(Stream stream)
        {
            var rawData = ReadRawDataFrom(stream);
            this.Name = rawData.Name;
            this.Data = BitConverter.ToInt32(rawData.DataBuffer, 0);
        }

        public override void WriteTo(Stream stream)
        {
            var rawData = new StreamRawData();
            rawData.Name = this.Name;
            rawData.DataBuffer = BitConverter.GetBytes(this.Data);

            WriteRawDataTo(stream, rawData);
        }
    }
}
