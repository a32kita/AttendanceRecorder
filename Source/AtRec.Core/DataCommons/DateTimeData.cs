using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtRec.Core.DataCommons.StockableDataExtensions;

namespace AtRec.Core.DataCommons
{
    [StockableData("{D70B2357-9D58-4661-A2CA-826DE2CC3F39}")]
    public class DateTimeData : StockData<DateTime>
    {
        public override void ReadFrom(Stream stream)
        {
            var rawData = ReadRawDataFrom(stream);

            var ms = new MemoryStream(rawData.DataBuffer);
            var data = new DateTime();
            using (var br = new BinaryReader(ms))
            {
                var dateTimeDatas = new int[]
                {
                    br.ReadInt32(),
                    br.ReadInt32(),
                    br.ReadInt32(),
                    br.ReadInt32(),
                    br.ReadInt32(),
                    br.ReadInt32(),
                    br.ReadInt32()
                };

                data = new DateTime(
                    dateTimeDatas[0],
                    dateTimeDatas[1],
                    dateTimeDatas[2],
                    dateTimeDatas[3],
                    dateTimeDatas[4],
                    dateTimeDatas[5],
                    dateTimeDatas[6] );
            }

            this.Name = rawData.Name;
            this.Data = data;
        }

        public override void WriteTo(Stream stream)
        {
            var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(this.Data.Year);
                bw.Write(this.Data.Month);
                bw.Write(this.Data.Day);
                bw.Write(this.Data.Hour);
                bw.Write(this.Data.Minute);
                bw.Write(this.Data.Second);
                bw.Write(this.Data.Millisecond);
            }

            var rawData = new StreamRawData();
            rawData.Name = this.Name;
            rawData.DataBuffer = ms.ToArray();
            WriteRawDataTo(stream, rawData);
        }
    }
}
