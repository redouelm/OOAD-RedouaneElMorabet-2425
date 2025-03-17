using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKaartspel
{
    internal class Kaart
    {
        public int Nummer { get; }
        public char Kleur { get; }

        public Kaart(int nmr, char klr)
        {
            if (nmr < 1 || nmr > 13)
            {
                throw new ArgumentException("Nummer moet tussen 1 en 13 liggen.");
            }

            if (klr != 'C' && klr != 'S' && klr != 'H' && klr != 'D')
            {
                throw new ArgumentException("Kleur moet 'C', 'S', 'H' of 'D' zijn.");
            }

            Nummer = nmr;
            Kleur = klr;
        }
        public override string ToString()
        {
            string kleurVolledig = Kleur switch
            {
                'C' => "Clubs",
                'S' => "Spades",
                'H' => "Hearts",
                'D' => "Diamonds",
                _ => "Onbekend"
            };

            return $"{Nummer} van {kleurVolledig}";
        }
    }
}
