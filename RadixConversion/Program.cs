using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;


namespace RadixConversion
{
    public enum BaseNumber
    {
        Binary = 2,
        Octal = 8,
        Decimal = 10,
        Hexadecimal = 16,

    }

    //拡張メソッド
    //(int)がダサいと思い作成
    static class Conversion
    {
        public static string ToString(this int i, BaseNumber j)
        {
            return Convert.ToString(i, (int)j);
        }

        public static int ToInt(this int? i)
        {
            return (int)i;
        }

    }

    //Listを作成し、Listの要素を基数変換するclass
    public class Numbers
    {
        private List<string> nums;
        private int lower;
        private int upper;

        public Numbers(int? l ,int? u)
        {
            int lower = l.HasValue ? l.ToInt() : 0;

            int upper = u.HasValue ? u.ToInt() : 10;

            this.lower = lower;
            this.upper = upper;
            this.nums = Enumerable.Range(lower, upper).Select(x => x.ToString()).ToList();
        }

        private Numbers(Numbers numbers, int? i)
        {
            int j = i.HasValue ? i.ToInt() : 1;

            var tmp = numbers.Nums.Select(x => Convert.ToInt32(x)).ToList();

            this.nums = tmp.Select(x => Convert.ToString(x * i)).ToList();
        }

        // インデクサー
        public string this[int i]
        {
            get => nums[i];
        }


        //setは基本的にクラス内でしか使用しない
        public List<string> Nums
        {
            get =>this.nums;
        }

        public int Lower
        {
            get =>this.lower;
        }

        public int Upper
        {
            get => this.upper;
        }


        //基数変換を行う
        public void NumberConversion(BaseNumber i)
        {
            var tmp = this.Nums.Select(x => Convert.ToInt32(x)).ToList();
            this.nums = tmp.Select(x => x.ToString(i)).ToList();
        }

        // 演算子オーバーロード
        //各要素をi倍させる
        //進数変換前に使用すること
        public static Numbers operator *(Numbers numbers, int i) => 
            new Numbers(numbers, i);

        //各要素にiを足す
        //進数変換前に使用すること
        public static Numbers operator +(Numbers numbers, int i) =>
            new Numbers(numbers.Lower + i, numbers.Upper);

    }

    class Program
    {
        static void Main(string[] args)
        {

            //10進数
            Numbers numbers1 = new Numbers(1, 10);

            var decimalNumbers = numbers1 + 2;

            //for版
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(decimalNumbers[i]);
            }

            //foreach版
            foreach (string i in decimalNumbers.Nums)
            {
                Console.WriteLine(i);
            }

            //ForEach()版
            decimalNumbers.Nums.ForEach(i => Console.WriteLine(i));

            //2進数
            Numbers binaryNumbers = new Numbers(1, 10);

            binaryNumbers.NumberConversion(BaseNumber.Binary);

            binaryNumbers.Nums.ForEach(i => Console.WriteLine(i));


            //8進数
            Numbers numbers3 = new Numbers(1, 10);

            var octalNumbers = numbers3 * 2;

            octalNumbers.NumberConversion(BaseNumber.Octal);

            octalNumbers.Nums.ForEach(i => Console.WriteLine(i));


            //16進数
            Numbers hexadecimal = new Numbers(1, 10);

            hexadecimal.NumberConversion(BaseNumber.Hexadecimal);

            hexadecimal.Nums.ForEach(i => Console.WriteLine(i));
        }
    }
}
