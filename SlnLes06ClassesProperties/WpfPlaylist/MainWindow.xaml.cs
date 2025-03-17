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
        Song song;
        public MainWindow()
        {
            InitializeComponent();
            LoadArtists();
            LoadSongs();
            LstBoxSongs.ItemsSource = songs;
        }
        private void LoadArtists()
        {
            string excelPath = "WpfPlaylistData.xlsx";
            var workbook = new XLWorkbook(excelPath);
            var worksheet = workbook.Worksheets.First();

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                string name = row.Cell(1).GetString();
                DateTime birthDate = DateTime.ParseExact(row.Cell(2).GetString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string bio = row.Cell(3).GetString();
                string imagePath = row.Cell(4).GetString();

                artists.Add(new Artist(name, birthDate, bio, imagePath));
            }
        }
        private void LoadSongs()
        {
            string mp3Folder = "Mp3";
            var mp3Files = Directory.GetFiles(mp3Folder, "*.mp3");

            foreach (var file in mp3Files)
            {
                var tagFile = TagLib.File.Create(file);
                string songName = tagFile.Tag.Title;
                string artistName = tagFile.Tag.FirstPerformer;
                int year = (int)(tagFile.Tag.Year);
                TimeSpan duration = tagFile.Properties.Duration;

                var artist = artists.FirstOrDefault(a => a.Name.Equals(artistName, StringComparison.OrdinalIgnoreCase));
                if (artist == null)
                {
                    artist = new Artist(artistName, DateTime.MinValue, "Onbekende artiest", "");
                    artists.Add(artist);
                }

                var song = new Song(songName, artist, year, duration, new Uri(System.IO.Path.GetFullPath(file)));
                songs.Add(song);
            }
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
        }
        private void StopSong()
        {
            mediaPlayer.Stop();
            txtStatusSong.Text = "Playback stopped.";
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (currentSong != null)
                PlaySong(currentSong);
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
            txtBirthDay.Text = $"born {artist.BirthDateString}";
            txtBio.Text = artist.Bio;

            if (File.Exists(artist.ImagePath))
            {
                imgbxArtist.Source = new BitmapImage(new Uri(Path.GetFullPath(artist.ImagePath)));
            }
            else
            {
                imgbxArtist.Source = null; // Of default afbeelding
            }
        }
    }
}
