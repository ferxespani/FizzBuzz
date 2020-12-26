using System;
using System.Collections.Generic;

namespace BTS
{
    class Program
    {
        static void Main(string[] args)
        {
            BinaryTree<int> tree = new BinaryTree<int>();

            tree.Add(5);
            tree.Add(7);
            tree.Add(1);
            tree.Add(2);
            tree.Add(9);

            List<int> list = tree.Traverse();

            foreach (int i in list)
                Console.Write($"{i}, ");
        }
    }
}
