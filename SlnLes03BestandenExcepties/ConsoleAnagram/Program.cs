using System.Security.AccessControl;

namespace ConsoleAnagram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stack<string> okWoorden = new Stack<string>();
            int aantalLetters;
            string inhoud;
            Random random = new Random();

            //string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = System.IO.Path.Combine(@".../files/1000woordern.txt");



            Console.WriteLine("CONSOLE ANGRAM");
            Console.WriteLine("==============");
            Console.WriteLine("");
            Console.Write("Kies het aantal letters (5-15): ");
            aantalLetters = Convert.ToInt32(Console.ReadLine());

            // lees tekstinhoud bestand in
            inhoud = File.ReadAllText(filePath);
            string[] woorden = inhoud.Split('\n');

            // woorden filteren  
            foreach (string woord in woorden)
            {
                string refreshWoord = woord.Trim().ToLower();
                if (refreshWoord.Length == aantalLetters)
                {
                    okWoorden.Push(refreshWoord);
                }
            }

            // woord kiezen
            int randomGetal = random.Next(okWoorden.Count);

            string gekozenWoord = okWoorden.ElementAt(randomGetal);

            Console.WriteLine("");

            // nieuwe anagram of ok
            string anagram;
             
            do
            {
                anagram = new string(gekozenWoord.ToCharArray().OrderBy(_ => random.Next()).ToArray()); ;
                Console.WriteLine($"Anagram: {anagram}");
                Console.Write("Het woord (druk op enter om opnieuw het woord te schudden): ");
                

            } while (string.IsNullOrEmpty(Console.ReadLine()));

            string input = Console.ReadLine();

            if (input == gekozenWoord)
            {
                Console.WriteLine("proficiat! je hebt het woord geraden");
            }
            else
            {
                Console.WriteLine($"helaas! Het woord was '{gekozenWoord}'");
            }

        }
    }
}
