using System;
using System.Collections.Generic;
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

namespace WpfComplexiteit
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string input = txtInput.Text;

            int aantalKarakters = input.Length;
            int lettergrepen = AantalLettergrepen(input);
            double complexi = Complexiteit(input);
            txtBkOutput.Text = $@"Aantal karakters: {aantalKarakters}
Aantal lettergrepen: {lettergrepen}
Complexiteit: {Math.Round(complexi, 1)}";
        }

        static bool IsKlinker(char a)
        {
            return "aeiouAEIOU".Contains(a);
        }
        static int AantalLettergrepen(string input)
        {
            int lettergrepen = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (IsKlinker(input[i]))
                {
                    if (i == 0 || !IsKlinker(input[i - 1]))
                    {
                        lettergrepen++;
                    }
                }
            }
            return lettergrepen;
        }
        static double Complexiteit(string input)
        {
            double totaal = input.Length / 3.0 + AantalLettergrepen(input);

            for (int i = 0; i < input.Length;)
            {
                if ("xyq".Contains(input[i]))
                {
                    totaal++;
                    i++;
                }
                else
                {
                    i++;
                }
            }
            return totaal;
        }
    }
}
