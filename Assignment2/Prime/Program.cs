using System;

class Program
{
    static void Main()
    {
        Console.Write("请输入一个正整数：");
        int num = int.Parse(Console.ReadLine());

        Console.Write($"{num} 的所有素数因子是：");

        for (int i = 2; i <= num; i++)
        {
            while (num % i == 0 && IsPrime(i))
            {
                Console.Write($"{i} ");
                num /= i;
            }
        }

        Console.WriteLine();
    }

    // 判断一个数是否为素数
    static bool IsPrime(int n)
    {
        if (n < 2)
            return false;
        for (int i = 2; i <= Math.Sqrt(n); i++)
        {
            if (n % i == 0)
                return false;
        }
        return true;
    }
}
