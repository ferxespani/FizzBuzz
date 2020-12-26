using System;

namespace Anagram
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Enter value1: ");
                string testValue1 = Console.ReadLine();
                Console.WriteLine("Enter value2: ");
                string testValue2 = Console.ReadLine();

                if (IsAnagram(testValue1, testValue2))
                    Console.WriteLine("It is anagram");
                else
                    Console.WriteLine("It isn`t anagram");
                Console.WriteLine("Enter 'q' to exit");
            } while (Console.ReadLine() != "q");
        }

        static bool IsAnagram(string value1, string value2)
        {
            int counter = 0;

            for (int i = 0; i < value2.Length; i++)
            {
                if (value1.Contains(value2[i]))
                {
                    counter++;
                }
            }

            if (counter == value1.Length)
                return true;

            return false;

        }
    }
}
