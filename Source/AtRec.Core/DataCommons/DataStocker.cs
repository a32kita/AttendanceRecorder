using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtRec.Core.DataCommons.StockableDataExtensions;

namespace AtRec.Core.DataCommons
{
    public class DataStocker
    {
        // 非公開フィールド
        private List<IStockableData> _stocks;


        // 非公開静的フィールド
        private static Encoding _fileEncoding;
        private static byte[] _fileMagicNumberBuf;
        private static Dictionary<Guid, Type> _readableDataTypes;


        // 公開プロパティ

        public IList<IStockableData> Stocks
        {
            get => _stocks;
        }


        // コンストラクタ

        /// <summary>
        /// <see cref="DataStocker"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public DataStocker()
        {
            this._stocks = new List<IStockableData>();
        }


        // 静的コンストラクタ

        static DataStocker()
        {
            _fileEncoding = Encoding.ASCII;
            _fileMagicNumberBuf = _fileEncoding.GetBytes("DSP gbex18 ");
            _readableDataTypes = new Dictionary<Guid, Type>();

            // 型ロード
            registDefinedTypes();
        }


        // 非公開静的メソッド

        private static void registDefinedTypes()
        {
            var dataTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(type => type.Namespace != null && type.Namespace.Contains("AtRec.Core.DataCommons") && type.IsValidStockableType());
            foreach (var type in dataTypes)
                RegistStockableType(type);
        }


        // 限定公開静的メソッド

        internal static void RegistStockableType(Type stockableType)
        {
            if (!stockableType.IsValidStockableType())
                throw new NotSupportedException();

            _readableDataTypes.Add(stockableType.GetStockableDataTypeId(), stockableType);
        }


        // 公開メソッド

        /// <summary>
        /// 現在の中で指定された名前を持つデータを全て返します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<object> GetDatas(string name)
        {
            return this._stocks.Where(item => item.Name == name).Select(item => item.GetData());
        }

        /// <summary>
        /// 現在のストックの中で指定された名前を持つ最初のデータを返します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetData(string name)
        {
            var datas = this.GetDatas(name);
            if (datas.Count() == 0)
                throw new KeyNotFoundException();
            return datas.First();
        }

        /// <summary>
        /// 現在のストックの中で指定された名前を持つ最初のデータを型引数で指定された型で返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetData<T>(string name)
        {
            return (T)this.GetData(name);
        }

        public void WriteTo(Stream stream)
        {
            using (var bw = new BinaryWriter(stream, _fileEncoding, true))
            {
                bw.Write(_fileMagicNumberBuf);
                bw.Write((uint)this._stocks.Count);
                foreach (var stock in this._stocks)
                {
                    var typeIdBuf = stock.GetType().GetStockableDataTypeId().ToByteArray();
                    bw.Write((ushort)typeIdBuf.Length);
                    bw.Write(typeIdBuf);
                    bw.Flush();

                    stock.WriteTo(stream);
                }

                bw.Flush();
            }
        }


        // 公開静的メソッド

        public static DataStocker ReadFrom(Stream stream)
        {
            using (var br = new BinaryReader(stream, _fileEncoding, true))
            {
                if (!br.ReadBytes(_fileMagicNumberBuf.Length).SequenceEqual(_fileMagicNumberBuf))
                    throw new NotSupportedException();

                var result = new DataStocker();
                var dataCount = br.ReadUInt32();
                for (var i = 0u; i < dataCount; i++)
                {
                    var typeIdBufLen = br.ReadUInt16();
                    var typeId = new Guid(br.ReadBytes(typeIdBufLen));

                    if (!_readableDataTypes.ContainsKey(typeId))
                        throw new NotSupportedException("サポートしていない型のデータが存在します。 TypeId: " + typeId.ToString());

                    var type = _readableDataTypes[typeId];
                    var data = (IStockableData)Activator.CreateInstance(type);
                    data.ReadFrom(stream);

                    result.Stocks.Add(data);
                }

                return result;
            }
        }
    }
}
