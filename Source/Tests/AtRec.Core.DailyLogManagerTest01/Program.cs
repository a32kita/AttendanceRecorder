using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtRec.Core;
using AtRec.Core.DataCommons;
using AtRec.Core.Entities;

namespace AtRec.Core.DailyLogManagerTest01
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine("== SAVE TEST ==");
            SaveTest();
            Console.WriteLine();
            
            Console.WriteLine("== LOAD TEST ==");
            LoadTest();
            Console.WriteLine();

            Console.ReadKey();
        }

        public static void SaveTest()
        {
            // 正常系

            var yesterdayDailyLog = DailyLog.LoadExistOrCreate(DateTime.Today - TimeSpan.FromDays(1));
            if (yesterdayDailyLog.Records.Count != 0)
                Console.WriteLine("すでに {0} 件のレコードが存在しております。", yesterdayDailyLog.Records.Count);
            yesterdayDailyLog.AddRecord(new TimeRecord() { Time = DateTime.Now - TimeSpan.FromDays(1) });
            DailyLogManager.SaveDailyLog(yesterdayDailyLog);

            var todayDailyLog = DailyLog.LoadExistOrCreate(DateTime.Today);
            if (todayDailyLog.Records.Count != 0)
                Console.WriteLine("すでに {0} 件のレコードが存在しております。", todayDailyLog.Records.Count);
            todayDailyLog.AddRecord(new TimeRecord() { Time = DateTime.Now });
            DailyLogManager.SaveDailyLog(todayDailyLog);


            // 異常系
#if FALSE
            var errorDailyLog = new DailyLog(DateTime.Today - TimeSpan.FromDays(2));
            errorDailyLog.AddRecord(new TimeRecord() { Time = DateTime.Now - TimeSpan.FromDays(3) });
#endif
        }

        public static void LoadTest()
        {
            var result = DailyLogManager.LoadPriod(DateTime.Today - TimeSpan.FromDays(1), DateTime.Today + TimeSpan.FromDays(1));
            foreach (var log in result)
            {
                Console.WriteLine("[Date={0}]", log.TargetDate);
                foreach (var record in log.Records)
                    Console.WriteLine("* {0:00}:{1:00}:{2:00}", record.Time.Hour, record.Time.Minute, record.Time.Second);
            }
        }
    }
}
