using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtRec.Core.DataCommons
{
    public abstract class StockData<T> : IStockableData
    {
        // 公開プロパティ

        /// <summary>
        /// データの名前を取得または設定します。
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// <see cref="T"/> 型のデータを取得または設定します。
        /// </summary>
        public T Data
        {
            get;
            set;
        }


        // コンストラクタ

        /// <summary>
        /// <see cref="StockData{T}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        internal StockData()
        {
            // 実装なし
        }


        // 限定公開静的メソッド
        
        protected static void WriteRawDataTo(Stream stream, StreamRawData rawData)
        {
            using (var bw = new BinaryWriter(stream, Encoding.Default, true))
            {
                var nameBuffer = Encoding.UTF8.GetBytes(rawData.Name);
                bw.Write((UInt16)nameBuffer.Length);
                bw.Write(nameBuffer);

                bw.Write((UInt64)rawData.DataBuffer.Length);
                bw.Write(rawData.DataBuffer);
            }
        }

        protected static StreamRawData ReadRawDataFrom(Stream stream)
        {
            using (var br = new BinaryReader(stream, Encoding.Default, true))
            {
                var nameBufferLength = br.ReadUInt16();
                var nameBuffer = br.ReadBytes(nameBufferLength);

                var dataBufferLength = br.ReadUInt64();
                var dataBuffer = br.ReadBytes((int)dataBufferLength);

                return new StreamRawData() { Name = Encoding.UTF8.GetString(nameBuffer), DataBuffer = dataBuffer };
            }
        }


        // 公開メソッド

        /// <summary>
        /// 現在のデータをストリームへ書き出します。
        /// </summary>
        /// <param name="stream"></param>
        public abstract void WriteTo(Stream stream);

        /// <summary>
        /// 現在のデータへストリームから読み込みます。
        /// </summary>
        /// <param name="stream"></param>
        public abstract void ReadFrom(Stream stream);

        /// <summary>
        /// データを <see cref="object"/> 型で取得します。
        /// </summary>
        /// <returns></returns>
        public virtual object GetData()
        {
            return this.Data;
        }


        // 内部クラス

        protected class StreamRawData
        {
            public string Name
            {
                get;
                set;
            }

            public byte[] DataBuffer
            {
                get;
                set;
            }
        }
    }
}
