using LikedList;
using System;

namespace LinkedList
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkedList<int> myList = new LinkedList<int>(5, 6, 7, 8);

            myList.Add(78);
            myList.Insert(2, 44);

            foreach (int i in myList)
                Console.WriteLine(i);
        }
    }
}
