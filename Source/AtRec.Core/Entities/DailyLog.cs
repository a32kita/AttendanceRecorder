using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtRec.Core.Entities
{
    public class DailyLog
    {
        // 非公開フィールド
        private DateTime _targetDate;
        private List<TimeRecord> _records;


        // 公開プロパティ

        public DateTime TargetDate
        {
            get => this._targetDate;
        }

        /// <summary>
        /// この日に記録されたレコードの一覧を取得します。
        /// </summary>
        public IReadOnlyCollection<TimeRecord> Records
        {
            get => this._records;
        }


        // コンストラクタ

        /// <summary>
        /// <see cref="DailyLog"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="targetDate"></param>
        internal DailyLog(DateTime targetDate)
        {
            this._targetDate = targetDate;
            this._records = new List<TimeRecord>();
        }


        // 公開メソッド

        /// <summary>
        /// 新しい <see cref="TimeRecord"/> を加えます。
        /// </summary>
        /// <param name="record"></param>
        public void AddRecord(TimeRecord record)
        {
            if (record.Time.Date != this._targetDate.Date)
                throw new InvalidOperationException("日付が一致しません。");

            this._records.Add(record);
        }


        // 公開静的メソッド

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DailyLog LoadExistOrCreate(DateTime date)
        {
            return DailyLogManager.Load(date);
        }
    }
}
