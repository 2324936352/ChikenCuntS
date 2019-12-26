using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChikenCuntS
{
    class Program
    {
        static void Main(string[] args)
        {
            //公鸡每只5元，母鸡每只3元，三只小鸡1元，用100元买100只鸡，问公鸡、母鸡、小鸡各多少只
            Console.WriteLine("Hello World!");
            //假设每只鸡一元（1只母鸡加3只小鸡4元）

            //循环自增加3得小鸡的价格，根据假设在下面除3
            for (int i = 3; i < 100; i += 3)
            {
                //循环自增加1得母鸡的价格根据假设乘3
                for (int j = 1; j < 100; j++)
                {
                    //根据上面的假设，利用判断语句，如果母鸡加小鸡的只数，和100减去母鸡 i 和小鸡 j 的只数的结果乘5（公鸡的单价）刚好得到100 即输出结果 且100减 i 和 j必须大于 0 （不这样公鸡会得到负数）
                    if (i / 3 + j * 3 + (100 - i - j) * 5 == 100 && 100 - i - j > 0)//
                    {
                        Console.WriteLine("小鸡 " + i + "\t母鸡 " + j + "\t公鸡 " + (100 - i - j));
                    }
                }
            }
        }
    }
}
