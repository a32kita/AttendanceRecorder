using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtRec.Core.DataCommons.StockableDataExtensions;

namespace AtRec.Core.DataCommons
{
    [StockableData("{C34E4141-CAAD-4C1A-A7BC-E5F9A382641D}")]
    public class GuidData : StockData<Guid>
    {
        public override void ReadFrom(Stream stream)
        {
            var rawData = ReadRawDataFrom(stream);
            this.Name = rawData.Name;
            this.Data = new Guid(rawData.DataBuffer);
        }

        public override void WriteTo(Stream stream)
        {
            var rawData = new StreamRawData();
            rawData.Name = this.Name;
            rawData.DataBuffer = this.Data.ToByteArray();

            WriteRawDataTo(stream, rawData);
        }
    }
}
