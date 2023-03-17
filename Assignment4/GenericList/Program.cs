using System;

namespace GenericList
{
    class Program
    {
        static void Main(string[] args)
        {
            GenericList<int> list = new GenericList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);

            // Print the list elements
            Console.WriteLine("List elements:");
            list.ForEach(i => Console.Write(i + " "));
            Console.WriteLine();

            // Compute the maximum value
            int max = list.Head.Data;
            list.ForEach(i => { if (i > max) max = i; });
            Console.WriteLine("Maximum value: " + max);

            // Compute the minimum value
            int min = list.Head.Data;
            list.ForEach(i => { if (i < min) min = i; });
            Console.WriteLine("Minimum value: " + min);

            // Compute the sum of the elements
            int sum = 0;
            list.ForEach(i => sum += i);
            Console.WriteLine("Sum of the elements: " + sum);
        }
    }

    public class GenericList<T>
    {
        private Node<T> head;
        private Node<T> tail;

        public Node<T> Head
        {
            get { return head; }
        }

        public void Add(T t)
        {
            Node<T> n = new Node<T>(t);
            if (tail == null)
            {
                head = tail = n;
            }
            else
            {
                tail.Next = n;
                tail = n;
            }
        }

        public void ForEach(Action<T> action)
        {
            for (Node<T> node = head; node != null; node = node.Next)
            {
                action(node.Data);
            }
        }
    }

    public class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Next { get; set; }

        public Node(T data)
        {
            Data = data;
            Next = null;
        }
    }
}
