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
using System.Windows.Threading;

namespace WpfEllipsen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {


            InitializeComponent();
            // maak de ellips
            Ellipse newEllipse = new Ellipse()
            {
                Width = 150,
                Height = 60,
                Fill = new SolidColorBrush(Color.FromRgb(122, 78, 200))
            };
            double xPos = 50;
            double yPos = 85;
            newEllipse.SetValue(Canvas.LeftProperty, xPos);
            newEllipse.SetValue(Canvas.TopProperty, yPos);
            //voeg ellips toe aan het canvas
            canvas1.Children.Add(newEllipse);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            canvas1.Children.Clear();

            Random rnd = new Random();
            const int MaxEllipsen = 25;
            
            const int Min = 20;
            const int Max = 100;

            const int MinPosX = 0;
            const int MaxPosX = 700;

            const int MinPosY = 0;
            const int MaxPosY= 700;

            const byte KleurMin = 0;
            const byte KleurMax = 255;

            for (int i = 0; i < MaxEllipsen; i++)
            {
                byte r01 = (byte)rnd.Next(KleurMin, KleurMax + 1);
                byte g01 = (byte)rnd.Next(KleurMin, KleurMax + 1);
                byte b01 = (byte)rnd.Next(KleurMin, KleurMax + 1);

                Ellipse newEllipse = new Ellipse()
                {
                    Width = rnd.Next(Min,Max + 1),
                    Height = rnd.Next(Min, Max + 1),
                    Fill = new SolidColorBrush(Color.FromRgb(r01, g01, b01))
                };
                

                double xPos = rnd.Next(MinPosX, MaxPosX);
                double yPos = rnd.Next(MinPosY, MaxPosY);
                newEllipse.SetValue(Canvas.LeftProperty, xPos);
                newEllipse.SetValue(Canvas.TopProperty, yPos);
                //voeg ellips toe aan het canvas
                canvas1.Children.Add(newEllipse);
            }

        }
    }
}
