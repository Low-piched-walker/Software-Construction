using System;

class Program
{
    static void Main()
    {
        bool[] isPrime = new bool[101]; // 定义一个长度为101的布尔数组，用于表示1~100的数是否为素数
        for (int i = 2; i <= 100; i++)
        {
            isPrime[i] = true; // 先假设所有数都是素数
        }

        for (int i = 2; i <= 100; i++)
        {
            if (isPrime[i]) // 如果i是素数
            {
                // 将i的倍数标记为非素数
                for (int j = 2 * i; j <= 100; j += i)
                {
                    isPrime[j] = false;
                }
            }
        }

        Console.WriteLine("2~100以内的素数为：");
        for (int i = 2; i <= 100; i++)
        {
            if (isPrime[i]) // 输出素数
            {
                Console.Write(i + " ");
            }
        }
        Console.WriteLine();
    }
}
