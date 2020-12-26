using System;

namespace Palindrom
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                string testValue = Console.ReadLine();

                if (IsPalindrom(testValue))
                    Console.WriteLine("It is palindrom");
                else
                    Console.WriteLine("It isn`t palindrom");
                Console.WriteLine("Enter 'q' to exit");
            } while (Console.ReadLine() != "q");
        }

        static bool IsPalindrom(string value)
        {
            int length = value.Length;
            int counter = 0;

            for (int i = 0; i < length; i++)
            {
                if (value[i] == value[length - 1 - i])
                    counter++;
                else
                    return false;

                if (counter >= length / 2)
                    return true;
            }

            return false;
        }
    }
}
