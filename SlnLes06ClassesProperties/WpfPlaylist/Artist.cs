using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPlaylist
{
    internal class Artist
    {
        public string Name { get; set; }               
        public DateTime BirthDate { get; set; }        
        public string Bio { get; set; }                
        public string ImagePath { get; set; }          

        // Constructor
        public Artist(string name, DateTime birthDate, string bio, string imagePath)
        {
            Name = name;
            BirthDate = birthDate;
            Bio = bio;
            ImagePath = imagePath;
        }

        // Extra: Handige weergave voor UI
        public string BirthDateString => BirthDate.ToString("dd/MM/yyyy");

        // Optioneel: override ToString() voor weergave in bv. comboBox
        public override string ToString()
        {
            return Name;
        }
    }
}
