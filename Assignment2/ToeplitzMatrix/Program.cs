using System;

class Program
{
    static void Main()
    {
        int[,] matrix = { { 1, 2, 3, 4 },
                          { 5, 1, 2, 3 },
                          { 9, 5, 1, 2 } }; // 假设该矩阵为 matrix

        bool isToeplitzMatrix = true;

        for (int i = 0; i < matrix.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < matrix.GetLength(1) - 1; j++)
            {
                if (matrix[i, j] != matrix[i + 1, j + 1]) // 如果有一条对角线上的元素不相同
                {
                    isToeplitzMatrix = false;
                    break;
                }
            }
        }

        Console.WriteLine($"该矩阵{(isToeplitzMatrix ? "是" : "不是")}托普利茨矩阵");
    }
}
