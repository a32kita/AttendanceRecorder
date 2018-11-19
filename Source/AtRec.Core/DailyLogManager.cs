using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtRec.Core.DataCommons;
using AtRec.Core.Entities;

namespace AtRec.Core
{
    public static class DailyLogManager
    {
        // 非公開フィールド

        private static readonly string DAT_EXTENSION = "adr";


        // 非公開静的メソッド

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetDate"></param>
        /// <returns></returns>
        private static string prepareDailyRecordsPath(DateTime targetDate)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + String.Format("\\AttendanceRecorder\\Records\\{0:0000}_{1:00}\\{1:00}{2:00}", targetDate.Year, targetDate.Month, targetDate.Day);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        private static string getRecordFilePath(TimeRecord record)
        {
            var path = prepareDailyRecordsPath(record.Time) + String.Format("\\{0:00}_{1:00}_{2:00}-{3:000}.{4}", record.Time.Hour, record.Time.Minute, record.Time.Second, record.Time.Millisecond, DAT_EXTENSION);
            return path;
        }


        // 公開静的メソッド

        /// <summary>
        /// 指定された <see cref="Stream"/> から <see cref="TimeRecord"/> を読み取ります。
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static TimeRecord LoadRecordFrom(Stream stream)
        {
            var result = new TimeRecord();
            var stocker = DataStocker.ReadFrom(stream);

            result.Time = stocker.GetData<DateTime>("RecordAt");
            result.Trigger = stocker.GetData<RecordTrigger>("Trigger");
            result.Flags = stocker.GetData<CheckFlags>("Flags");
            result.Memo = stocker.GetData<string>("Memo");

            return result;
        }

        public static DailyLog Load(DateTime date)
        {
            var dir = prepareDailyRecordsPath(date);
            var files = Directory.GetFiles(dir, "*." + DAT_EXTENSION, SearchOption.TopDirectoryOnly);
            var result = new DailyLog(date);

            foreach (var f in files)
            {
                using (var fs = File.OpenRead(f))
                    result.AddRecord(LoadRecordFrom(fs));
            }

            return result;
        }

        public static IEnumerable<DailyLog> LoadPriod(DateTime sinceDate, DateTime untilDate)
        {
            var currentDate = sinceDate;
            var resultLogs = new List<DailyLog>();
            while (currentDate <= untilDate)
            {
                resultLogs.Add(Load(currentDate));
                currentDate = currentDate.AddDays(1);
            }

            return resultLogs;
        }

        /// <summary>
        /// 指定された <see cref="Stream"/> へ <see cref="TimeRecord"/> を書き込みます。
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="record"></param>
        public static void SaveTimeRecord(Stream stream, TimeRecord record)
        {
            var stocker = new DataStocker();
            stocker.Stocks.Add(new ByteData() { Name = "TypeAuth", Data = new byte[] { 52, 44, 101, 87, 53, 50, 27, 74, 66, 4, 201, 76, 42 } });
            stocker.Stocks.Add(new DateTimeData() { Name = "RecordAt", Data = record.Time });
            stocker.Stocks.Add(new DateTimeData() { Name = "CreatedAt", Data = DateTime.Now });
            stocker.Stocks.Add(new GuidData() { Name = "Identifier", Data = Guid.NewGuid() });
            stocker.Stocks.Add(new IntegerData() { Name = "Trigger", Data = (int)record.Trigger });
            stocker.Stocks.Add(new IntegerData() { Name = "Flags", Data = (int)record.Flags });
            stocker.Stocks.Add(new StringData() { Name = "Memo", Data = record.Memo ?? "" });

            stocker.WriteTo(stream);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public static void SaveDailyLog(DailyLog log)
        {
            var dir = prepareDailyRecordsPath(log.TargetDate);
            foreach (var record in log.Records)
            {
                var path = getRecordFilePath(record);
                using (var fs = File.OpenWrite(path))
                    SaveTimeRecord(fs, record);
            }
        }
    }
}
