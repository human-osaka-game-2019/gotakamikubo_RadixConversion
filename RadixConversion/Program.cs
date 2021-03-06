﻿using System;
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

    public class Numbers
    {
        private List<string> nums;
        private int lower;
        private int upper;

        public Numbers(int? l ,int? u)
        {
            int lower = l.HasValue ? l.ToInt() : 0;

            int upper = u.HasValue ? u.ToInt() : 10;

            Lower = lower;
            Upper = upper;
            this.Nums = Enumerable.Range(lower, upper).Select(x => x.ToString()).ToList();
        }

        private Numbers(Numbers numbers, int? i)
        {
            int j = i.HasValue ? i.ToInt() : 1;

            var tmp = numbers.Nums.Select(x => Convert.ToInt32(x)).ToList();

            this.Nums = tmp.Select(x => Convert.ToString(x * i)).ToList();
        }

        // インデクサー
        public string this[int i]
        {
            get { return nums[i]; }
            private set { this.nums[i] = value; }
        }


        //setは基本的にクラス内でしか使用しない
        public List<string> Nums
        {
            get { return this.nums; }
            private set { this.nums = value; }
        }

        public int Lower
        {
            get { return this.lower; }
            private set { this.lower = value; }
        }

        public int Upper
        {
            get { return this.upper; }
            private set { this.upper = value; }
        }


        //進数変換を行う
        public void NumberConversion(BaseNumber i)
        {
            var tmp = this.Nums.Select(x => Convert.ToInt32(x)).ToList();
            this.Nums = tmp.Select(x => x.ToString(i)).ToList();
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

            //加算前 foreach版
            foreach (string i in numbers1.Nums)
            {
                Console.WriteLine(i);
            }

            var decimalNumbers = numbers1 + 2;

            //加算後 foreach版
            foreach(string i in decimalNumbers.Nums)
            {
                Console.WriteLine(i);
            }

            //加算後 for版
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(decimalNumbers[i]);
            }


            //2進数
            Numbers binaryNumbers = new Numbers(1, 10);

            binaryNumbers.NumberConversion(BaseNumber.Binary);

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(binaryNumbers[i]);
            }


            //8進数
            Numbers numbers3 = new Numbers(1, 10);

            var octalNumbers = numbers3 * 2;

            octalNumbers.NumberConversion(BaseNumber.Octal);

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(octalNumbers[i]);
            }


            //16進数
            Numbers hexadecimal = new Numbers(1, 10);

            hexadecimal.NumberConversion(BaseNumber.Hexadecimal);

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(hexadecimal[i]);
            }
        }
    }
}
