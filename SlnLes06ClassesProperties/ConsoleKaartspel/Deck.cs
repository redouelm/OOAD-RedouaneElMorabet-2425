using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKaartspel
{
    internal class Deck
    {
        public List<Kaart> Kaarten { get; private set; } = new List<Kaart>();
        private static char[] kleuren = { 'C', 'S', 'H', 'D' }; 
        public Deck()
        {
            // Maak alle 52 kaarten
            foreach (char kleur in kleuren)
            {
                for (int nummer = 1; nummer <= 13; nummer++)
                {
                    Kaarten.Add(new Kaart(nummer, kleur));
                }
            }
        }
        public void Schudden()
        {
            Random rnd = new Random();
            int n = Kaarten.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                (Kaarten[i], Kaarten[j]) = (Kaarten[j], Kaarten[i]);
            }
        }
        public Kaart NeemKaart()
        {
            if (Kaarten.Count == 0)
                throw new InvalidOperationException("Het deck is leeg!");

            Kaart getrokkenKaart = Kaarten[0];
            Kaarten.RemoveAt(0); 
            return getrokkenKaart;
        }
    }
}
