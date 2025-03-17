using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKaartspel
{
    internal class Speler
    {
        public string Naam { get; }
        public List<Kaart> Kaarten { get; set; }
        public bool HeeftNogKaarten => Kaarten.Count > 0;

        // Constructor
        public Speler(string naam)
        {
            Naam = naam;
            Kaarten = new List<Kaart>();
        }

        // Constructor 
        public Speler(string naam, List<Kaart> kaarten)
        {
            Naam = naam;
            Kaarten = kaarten ?? new List<Kaart>(); // Voorkomt null
        }

        // Methode om een willekeurige kaart te leggen
        public Kaart LegKaart()
        {
            if (Kaarten.Count == 0)
                throw new InvalidOperationException($"{Naam} heeft geen kaarten meer!");

            Random rng = new Random();
            int index = rng.Next(Kaarten.Count);
            Kaart gekozenKaart = Kaarten[index];
            Kaarten.RemoveAt(index); 
            return gekozenKaart;
        }
    }
}
