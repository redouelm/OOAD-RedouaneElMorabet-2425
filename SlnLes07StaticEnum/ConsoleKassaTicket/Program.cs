namespace ConsoleKassaTicket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Producten aanmaken 
                var product1 = new Product("bananen", 1.75m, "P02384");
                var product2 = new Product("brood", 2.10m, "P01820");
                var product3 = new Product("kaas", 3.99m, "P45612");
                var product4 = new Product("koffie", 4.10m, "P98754");

                // Ticket aanmaken met Visa betaalwijze
                var ticket = new Ticket("Annie", Ticket.Betaalwijze.Visa);
                ticket.VoegProductToe(product1);
                ticket.VoegProductToe(product2);
                ticket.VoegProductToe(product3);
                ticket.VoegProductToe(product4);

                // Ticket afdrukken
                ticket.DrukTicket();

                Console.ReadLine(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgetreden: {ex.Message}");
            }

            Console.ReadLine();
        }
    }
}