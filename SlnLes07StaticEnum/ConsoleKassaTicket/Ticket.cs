using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKassaTicket
{
    internal class Ticket
    {
        // Enum Betaalwijze binnen Ticket.cs
        public enum Betaalwijze
        {
            Visa,
            Cash,
            Bancontact
        }

        public List<Product> Producten { get; private set; }
        public Betaalwijze BetaaldMet { get; set; } = Betaalwijze.Cash; // Standaardwaarde is Cash
        public string Kassier { get; set; }
        public decimal TotaalPrijs { get { return BerekenTotaal(); } }
        public DateTime DatumTijd { get; private set; }

        public Ticket(string kassier, Betaalwijze betaaldMet = Betaalwijze.Cash)
        {
            Kassier = kassier;
            BetaaldMet = betaaldMet;
            Producten = new List<Product>();
            DatumTijd = DateTime.Now;
        }

        public void VoegProductToe(Product product)
        {
            Producten.Add(product);
        }

        private decimal BerekenTotaal()
        {
            decimal totaal = 0;
            foreach (var product in Producten)
            {
                totaal += product.EenheidsPrijs;
            }

            // Extra kosten toevoegen als met Visa wordt betaald
            if (BetaaldMet == Betaalwijze.Visa)
            {
                totaal += 0.12m;
            }

            return totaal;
        }

        public string PrintOut()
        {
            // Stringbuilder voor betere performance bij het samenvoegen van strings
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine("KASSATICKET");
            sb.AppendLine("============");
            sb.AppendLine($"Uw kassier: {Kassier}");
            sb.AppendLine();

            foreach (var product in Producten)
            {
                // Format: (P02384) bananen: 1,75
                sb.AppendLine($"({product.Code}) {product.Naam}: {product.EenheidsPrijs.ToString("F2").Replace(".", ",")}");
            }

            sb.AppendLine("------------");

            if (BetaaldMet == Betaalwijze.Visa)
            {
                sb.AppendLine($"Visa kosten: {0.12m.ToString("F2").Replace(".", ",")}");
            }

            // Format: Totaal: 12,06 (met komma als decimaalteken)
            sb.AppendLine($"Totaal: {TotaalPrijs.ToString("F2").Replace(".", ",")}");

            return sb.ToString();
        }

        public void DrukTicket()
        {
            Console.WriteLine(PrintOut());
        }
    }
}