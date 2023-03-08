using System;

class Program
{
    static void Main()
    {
        int[] nums = { 2, 5, 1, 7, 3, 9, 4, 6, 8 }; // 假设该整数数组为 nums

        int max = nums[0];  // 假设第一个元素为最大值
        int min = nums[0];  // 假设第一个元素为最小值
        int sum = 0;        // 初始值为 0，用于记录所有元素的和

        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[i] > max)
                max = nums[i];
            if (nums[i] < min)
                min = nums[i];
            sum += nums[i];
        }

        double avg = (double)sum / nums.Length;

        Console.WriteLine($"最大值：{max}");
        Console.WriteLine($"最小值：{min}");
        Console.WriteLine($"平均值：{avg}");
        Console.WriteLine($"所有元素的和：{sum}");
    }
}
