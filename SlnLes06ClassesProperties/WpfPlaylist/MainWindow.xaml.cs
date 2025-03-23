using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfPlaylist
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MediaPlayer mediaPlayer = new MediaPlayer();
        List<Artist> artists = new List<Artist>();
        List<Song> songs = new List<Song>();
        Song currentSong;
        public MainWindow()
        {
            InitializeComponent();
            LoadArtists();
            LoadSongs();
            LstBoxSongs.ItemsSource = songs;
        }
        private void LoadArtists()
        {
            artists = new List<Artist>
            {
                new Artist(
                 "Zakir Houssein",
                 new DateTime(1951, 3, 9),
                 "Ustad Zakir Hussain is an Indian tabla player, composer, percussionist, music producer and film actor.",
                 "zakir-houssein.jpg"),
                new Artist(
            "Anouar Brahem",
            new DateTime(1957, 10, 20),
            "Anouar Brahem is a Tunisian oud player and composer. He is widely acclaimed as an innovator in his field",
            "anouar-brahem.jpg"),
                
                new Artist(
            "Baaba Maal",
            new DateTime(1953, 6, 13),
            "Baaba Maal is a Senegalese singer and guitarist born in Podor, on the Senegal River. In addition to acoustic guitar, he also plays percussion.",
            "baaba-maal.jpg"),
                new Artist(
            "Mohammed Abdu",
            new DateTime(1949, 6, 12),
            "Mohammed Abdu is a Saudi singer who is renowned across the Middle East and Arab world. He has been described as “Artist of the Arabs”.",
            "mohammed-abdu.jpg")
            };
        }
        private void LoadSongs()
        {
            songs = new List<Song>
{
    new Song("La Belvedere Assiege", FindArtist("Anouar Brahem"), 1991, new TimeSpan(0, 4, 14), new Uri(System.IO.Path.GetFullPath("Mp3/belvedere-assiege-1m.mp3"), UriKind.Absolute)),

    new Song("Ronda", FindArtist("Anouar Brahem"), 1991, new TimeSpan(0, 3, 7), new Uri(System.IO.Path.GetFullPath("Mp3/ronda-1m.mp3"), UriKind.Absolute)),

    new Song("Raf Raf", FindArtist("Anouar Brahem"), 1991, new TimeSpan(0, 3, 33), new Uri(System.IO.Path.GetFullPath("Mp3/raf-raf-1m.mp3"), UriKind.Absolute)),

    new Song("The great indian desert", FindArtist("Zakir Houssein"), 2007, new TimeSpan(0, 6, 54), new Uri(System.IO.Path.GetFullPath("Mp3/great-indian-desert-1m.mp3"), UriKind.Absolute)),

    new Song("Yero Mama", FindArtist("Baaba Maal"), 1993, new TimeSpan(0, 4, 40), new Uri(System.IO.Path.GetFullPath("Mp3/yero-mama-1m.mp3"), UriKind.Absolute)),

    new Song("Samba", FindArtist("Baaba Maal"), 1993, new TimeSpan(0, 5, 43), new Uri(System.IO.Path.GetFullPath("Mp3/samba-1m.mp3"), UriKind.Absolute)),

    new Song("Habibi", FindArtist("Mohammed Abdu"), 2001, new TimeSpan(0, 11, 42), new Uri(System.IO.Path.GetFullPath("Mp3/habibi-1m.mp3"), UriKind.Absolute))
};
        }
        private Artist FindArtist(string name)
        {
            return artists.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstBoxSongs.SelectedItem is Song selectedSong)
            {
                PlaySong(selectedSong);
                ShowArtistInfo(selectedSong.Artist);
            }
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            txtStatusSong.Text += " STOPPED";
        }
        private void StopSong()
        {
            mediaPlayer.Stop();
            txtStatusSong.Text = "Playback stopped.";
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (LstBoxSongs.SelectedItem is Song selectedSong)
            {
                mediaPlayer.Open(selectedSong.Uri);
                mediaPlayer.Play();
                txtStatusSong.Text = $"Now playing: \"{selectedSong.Name}\" by {selectedSong.Artist.Name}";
            }
        }
        private void PlaySong(Song song)
        {
            currentSong = song;
            mediaPlayer.Open(song.Uri);
            mediaPlayer.Play();
            txtStatusSong.Text = $"Now playing: \"{song.Name}\" by {song.Artist.Name}";
        }

        private void ShowArtistInfo(Artist artist)
        {
            txtNameArtist.Text = artist.Name;
            txtBirthDay.Text = $"Born {artist.BirthDate:dd/MM/yyyy}";
            txtBio.Text = artist.Bio;
            imgbxArtist.Source = new BitmapImage(new Uri(artist.ImagePath, UriKind.Relative));
        }
    }
}
