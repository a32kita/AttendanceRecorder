using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtRec.Core.DataCommons.StockableDataExtensions;

namespace AtRec.Core.DataCommons
{
    [StockableData("{1C504911-9464-4474-A2F6-05261A3B6D6D}")]
    public class StringData : StockData<string>
    {
        private static Encoding _stringEncoding;

        static StringData()
        {
            //DataStocker.RegistStockableType(typeof(StringData));
            _stringEncoding = Encoding.UTF32;
        }

        public override void ReadFrom(Stream stream)
        {
            var rawData = ReadRawDataFrom(stream);
            this.Name = rawData.Name;
            this.Data = _stringEncoding.GetString(rawData.DataBuffer);
        }

        public override void WriteTo(Stream stream)
        {
            var rawData = new StreamRawData();
            rawData.Name = this.Name;
            rawData.DataBuffer = _stringEncoding.GetBytes(this.Data);

            WriteRawDataTo(stream, rawData);
        }
    }
}
