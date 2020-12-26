using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryForm
{
    public class Program
    {
        static void Main(string[] args)
        {
           for (int i = 0; i < 100; i++)
            {
                List<int> binary = ToBinary(i);

                Console.Write($"Binary form of { i }: ");

                for (int j = binary.Count - 1; j >= 0; j--)
                {
                    Console.Write(binary[j]);
                }

                Console.WriteLine();

                Console.WriteLine($"Max number: { MaxNumber(binary) }");
                binary = ToBinary(i);
                Console.WriteLine($"Min number: { MinNumber(binary) }");
                Console.WriteLine(new string('=', 80));
            }
        }

        static List<int> ToBinary(int value)
        {
            List<int> binaryNumbers = new List<int>();

            do
            {
                if (value % 2 == 0)
                    binaryNumbers.Add(0);
                else
                    binaryNumbers.Add(1);

                value /= 2;
            } 
            while (value != 0);


            return binaryNumbers;
        }

        static int FromBinary(List<int> binary)
        {
            int number = 0;

            for (int i = binary.Count - 1; i >= 0; i--)
            {
                number += binary[i] * (int)Math.Pow(2, i);
            }

            return number;
        }

        static int MinNumber(List<int> binaryForm)
        {
            int length = binaryForm.Count;

            int numberOfOnes = binaryForm.Where(i => i == 1).Sum();

            if (length == numberOfOnes)
                return FromBinary(binaryForm);
            else
            {
                for (int i = 0; i < length; i++)
                {
                    if (i < numberOfOnes)
                    {
                        binaryForm[i] = 1; 
                    }
                    else
                    {
                        binaryForm[i] = 0;
                    }
                }
                return FromBinary(binaryForm);
            }
        }

        static int MaxNumber(List<int> binaryForm)
        {
            int length = binaryForm.Count;

            int numberOfOnes = binaryForm.Where(i => i == 1).Sum();

            if (length == numberOfOnes)
                return FromBinary(binaryForm);
            else
            {
                for (int i = 0; i < length; i++)
                {
                    if (i > length - numberOfOnes - 1)
                    {
                        binaryForm[i] = 1;
                    }
                    else
                    {
                        binaryForm[i] = 0;
                    }
                }
                return FromBinary(binaryForm);
            }
        }
    }
}
