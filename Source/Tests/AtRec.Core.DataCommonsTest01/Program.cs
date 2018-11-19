using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtRec.Core.DataCommons;

namespace AtRec.Core.DataCommonsTest01
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var stocker = new DataStocker();
            stocker.Stocks.Add(new DateTimeData() { Name = "CreatedAt", Data = DateTime.Now });
            stocker.Stocks.Add(new StringData() { Name = "Data01", Data = "foo" });
            stocker.Stocks.Add(new StringData() { Name = "Data02", Data = "bar" });
            stocker.Stocks.Add(new StringData() { Name = "Data03", Data = "京成電鉄京成本線快速特急西馬込行き" });
            stocker.Stocks.Add(new StringData() { Name = "Data03", Data = "虹色の回路に無数の言葉たち半分だけでもここならば歩いて行ける突然のメロディまた加速していく心をつかんだ一枚の影も消し去る眼差しキラキラあまり大きくない道でも良いねそして駆け出す飛び込む奇跡へ見上げる手を振る光へ止まらない気持ちを繋いでゆくリフレクティア揺らめく近づく明日へ奏でる夢見る未来へまっさらな空どこまでも連れて涙の終わり合図に" });
            stocker.Stocks.Add(new IntegerData() { Name = "Data19", Data = 114514 });
            using (var fs = File.OpenWrite("temp.dat"))
                stocker.WriteTo(fs);

            using (var fs = File.OpenRead("temp.dat"))
            {
                var loadedStocker = DataStocker.ReadFrom(fs);
                Console.WriteLine("CreatedAt: {0}", loadedStocker.GetData("CreatedAt"));
                Console.WriteLine("Data01: {0}", loadedStocker.GetData("Data01"));
                Console.WriteLine("Data02: {0}", loadedStocker.GetData("Data02"));
                Console.WriteLine("Data03: {0}", loadedStocker.GetData("Data03"));
                Console.WriteLine("Data19: {0}", loadedStocker.GetData("Data19"));
            }

            Console.ReadKey();
        }
    }
}
