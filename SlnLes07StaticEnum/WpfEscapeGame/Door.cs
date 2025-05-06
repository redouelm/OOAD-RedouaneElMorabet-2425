using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    internal class Door
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsLocked { get; set; } = false;
        public Item Key { get; set; } // Optioneel: sleutel om deur te openen
        public Room ToRoom { get; set; } // De kamer waar deze deur naartoe leidt
        public Room Destination { get; internal set; }
        public string ImagePath { get; set; }


        public override string ToString()
        {
            return Name;
        }

        public bool TryUnlock(Item key)
        {
            if (!IsLocked) return true;
            if (Key != null && key == Key)
            {
                IsLocked = false;
                return true;
            }
            return false;
        }
    }
}

