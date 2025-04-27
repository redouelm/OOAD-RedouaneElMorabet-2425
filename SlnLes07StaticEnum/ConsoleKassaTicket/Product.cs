using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKassaTicket
{
    internal class Product
    {
        public string Naam { get; set; }
        public decimal EenheidsPrijs { get; set; }
        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                if (ValideerCode(value))
                {
                    _code = value.StartsWith("p") ? "P" + value.Substring(1) : value;
                }
                else
                {
                    throw new ArgumentException("Code moet uit 6 tekens bestaan en beginnen met 'P', bv. P45612");
                }
            }
        }

        public Product()
        {
            Naam = "";
            EenheidsPrijs = 0;
            _code = "P00000";
        }

        public Product(string naam, decimal eenheidsPrijs, string code)
        {
            Naam = naam;
            EenheidsPrijs = eenheidsPrijs;

            if (ValideerCode(code))
            {
                // Controleer de code begint met p
                _code = code.StartsWith("p") ? "P" + code.Substring(1) : code;
            }
            else
            {
                throw new ArgumentException("Code moet uit 6 tekens bestaan en beginnen met 'P', bv. P45612");
            }
        }

        public static bool ValideerCode(string code)
        {
            // Controleer de code 6 tekens en begint met p
            return code.Length == 6 && (code.StartsWith("P") || code.StartsWith("p"));
        }

        public override string ToString()
        {
            // Geeft stringvoorstelling terug, bv. "(P45612) bananen 1.24"
            return $"({Code}) {Naam} {EenheidsPrijs:F2}";
        }
    }
}

