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

namespace WpfTaken
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
            string Gekozenpersoon;
            private Stack<ListBoxItem> verwijderdeTaken = new Stack<ListBoxItem>();
            



        public MainWindow()
        {
            
            InitializeComponent();
            
            string gekozenPrioriteit = cmbPrioriteit.Text;
            string datum = DatDeadline.Text;


        }

        private void rbnAdam_Checked(object sender, RoutedEventArgs e)
        {
            Gekozenpersoon = rbnAdam.Content.ToString();
            CheckForm();

        }

        private void rbnBilal_Checked(object sender, RoutedEventArgs e)
        {
            Gekozenpersoon = rbnBilal.Content.ToString();


            CheckForm();
        }

        private void rbnChelsey_Checked(object sender, RoutedEventArgs e)
        {
            Gekozenpersoon = rbnChelsey.Content.ToString();
            CheckForm();


        }

        private void LstBxOverview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckForm();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(!CheckForm()) return;

            string line = $"{txtTaak.Text} (deadline: {DatDeadline.Text}; door: {Gekozenpersoon}) ";

            ListBoxItem item = new ListBoxItem();
            item.Content = line ;

            switch (cmbPrioriteit.Text)
            {
                case "laag":
                    item.Background = Brushes.LightSeaGreen;
                    break;
                case "gemiddeld":
                    item.Background = Brushes.LightYellow;
                    break;
                case "hoog":
                    item.Background = Brushes.LightCoral;
                    break;
            }

            LstBxOverview.Items.Add(item);

            txtTaak.Text = "";
            cmbPrioriteit.SelectedIndex = 0;
            DatDeadline.SelectedDate = null;
            rbnAdam.IsChecked = false;
            rbnBilal.IsChecked = false;
            rbnChelsey.IsChecked = false;
            Gekozenpersoon = null;
            txtError.Text = "";


        }

        private void txtTaak_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckForm();

        }

        private bool CheckForm()
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtTaak.Text))
                errors.AppendLine("Gelieve een taak in te vullen.");

            if (cmbPrioriteit.SelectedIndex <= 0)
                errors.AppendLine("Gelieve een prioriteit te kiezen.");

            if (DatDeadline.SelectedDate == null)
                errors.AppendLine("Gelieve een deadline te kiezen.");

            if (string.IsNullOrEmpty(Gekozenpersoon))
                errors.AppendLine("Gelieve een uitvoerder te kiezen.");

            if (errors.Length > 0)
            {
                txtError.Text = errors.ToString();
                return false;
            }

            txtError.Text = "";  // Foutmelding wissen als alles correct is
            return true;
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            if (verwijderdeTaken.Count > 0)
            {
                ListBoxItem hersteldItem = verwijderdeTaken.Pop();
                LstBxOverview.Items.Add(hersteldItem);
            }

        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (LstBxOverview.SelectedItem is ListBoxItem selectedItem)
            {
                LstBxOverview.Items.Remove(selectedItem);
                verwijderdeTaken.Push(selectedItem);
            }
        }
    }
}
