using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtRec.Core.DataCommons.StockableDataExtensions;

namespace AtRec.Core.DataCommons
{
    [StockableData("{20769F97-ECF2-4D20-BBE3-A52FB8BED63E}")]
    public class ByteData : StockData<byte[]>
    {
        public override void ReadFrom(Stream stream)
        {
            var rawData = ReadRawDataFrom(stream);
            this.Name = rawData.Name;
            this.Data = rawData.DataBuffer;
        }

        public override void WriteTo(Stream stream)
        {
            var rawData = new StreamRawData();
            rawData.Name = this.Name;
            rawData.DataBuffer = this.Data;

            WriteRawDataTo(stream, rawData);
        }
    }
}
