using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPlaylist
{
    internal class Song
    {
        public string Name { get; set; }              
        public Artist Artist { get; set; }            
        public int Year { get; set; }                
        TimeSpan Duration { get; set; }       
        public Uri Uri { get; set; }
        public Song(string name, Artist artist, int year, TimeSpan duration, Uri uri)
        {
            Name = name;
            Artist = artist;
            Year = year;
            Duration = duration;
            Uri = uri;
        }
        public override string ToString()
        {
            return $"{Name} - {Artist.Name} ({Year}, {Duration.Minutes}m{Duration.Seconds}s)";
        }
    }
}
