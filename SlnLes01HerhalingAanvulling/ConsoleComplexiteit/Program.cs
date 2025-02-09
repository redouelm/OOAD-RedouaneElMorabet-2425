namespace ConsoleComplexiteit
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            string input;

            do
            {
                Console.Write("Geef een woord (enter om  te stoppen): ");
                input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    // karakters tellen + resultaat
                    Console.WriteLine($"aantal karakters: {input.Length}");

                    // lettergrepen tellen + resultaat
                    int lettergrepen = AantalLettergrepen(input);
                    Console.WriteLine($"aantal lettergrepen: {lettergrepen}");

                    // complexitet berekenen + resultaat
                    double complexi = Complexiteit(input);
                    Console.WriteLine($"Complexiteit: {Math.Round(complexi, 1)}");
                    Console.WriteLine(" ");
                }
                else
                {
                    Console.WriteLine("");
                }
            }
            while (!string.IsNullOrEmpty(input));

            Console.WriteLine("Bedankt en tot ziens!");
        }

        static bool IsKlinker(char a)
        {
            return "aeiouAEIOU".Contains(a);
        }

        static int AantalLettergrepen(string input)
        {
            int lettergrepen = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (IsKlinker(input[i]))
                {
                    if (i == 0 || !IsKlinker(input[i - 1]))
                    {
                        lettergrepen++;
                    }
                }
            }
            return lettergrepen;

        }
        static double Complexiteit(string input)
        {
            double totaal = input.Length / 3.0 + AantalLettergrepen(input);

            for (int i = 0; i < input.Length;)
            {
                if ("xyq".Contains(input[i]))
                {
                    totaal++;
                    i++;
                }
                else
                {
                    i++;
                }
            }
            return totaal;
        }
    }
}
