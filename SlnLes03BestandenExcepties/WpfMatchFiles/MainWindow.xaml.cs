using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WpfMatchFiles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnChoos01_Click(object sender, RoutedEventArgs e)
        {
            txtPath01.Text = KiesDocument();
        }

        private void BtnChoos02_Click(object sender, RoutedEventArgs e)
        {
            txtPath02.Text = KiesDocument();

        }

        private void Vergelijk_Click(object sender, RoutedEventArgs e)
        {
            List<string> lijst1 = LeesTriplets(txtPath01.Text);
            List<string> lijst2 = LeesTriplets(txtPath02.Text);
            double overeenkomst = BerekenOvereenkomst(lijst1, lijst2);
            txtResultaat.Text = $"Overeenkomst: {overeenkomst} %";
        }

        private void txtPath01_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private string KiesDocument()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dialog.Filter = "Tekstbestanden|*.TXT;*.TEXT";
            string chosenFileName;
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                // user picked a file and pressed OK
                chosenFileName = dialog.FileName;
                return chosenFileName;
            }
            else
            {
                // user cancelled or escaped dialog window
                return string.Empty;
            }
            

        }

        private List<string> LeesTriplets(string path)
        {
            string content = File.ReadAllText(path).ToLower();
            content = Regex.Replace(content, "[^a-z]+", " ").Trim();
            string[] woorden = content.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);

            HashSet<string> triplets = new HashSet<string>();
            for (int i = 0; i < woorden.Length - 2; i++)
            {
                triplets.Add(woorden[i] + " " + woorden[i + 1] + " " + woorden[i + 2]);
            }
            return triplets.ToList();
        }

        private double BerekenOvereenkomst(List<string> lijst1, List<string> lijst2)
        {
            HashSet<string> set1 = new HashSet<string>(lijst1);
            HashSet<string> set2 = new HashSet<string>(lijst2);

            int gemeenschappelijk = set1.Intersect(set2).Count();
            int totaal = set1.Count + set2.Count;

            if (totaal == 0)
            {
                return 0;
            }
            return (2.0 * gemeenschappelijk / totaal) * 100;
        }
    }
}
