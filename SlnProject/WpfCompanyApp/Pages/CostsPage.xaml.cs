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
using CLBenchmark;

namespace WpfCompanyApp.Pages
{
    /// <summary>
    /// Interaction logic for CostsPage.xaml
    /// </summary>
    public partial class CostsPage : Page
    {
        private Company _loggedInCompany;
        CostRepository repo = new CostRepository();
        public CostsPage(Company loggedInCompany)
        {
            InitializeComponent();
            _loggedInCompany = loggedInCompany;
            LoadFilters();
            // LoadCosts();
        }
        //private void LoadCosts()
        //{
        //    List<Cost> kosten = repo.GetCostsForCompany(_loggedInCompany.Id);

        //    foreach (Cost cost in kosten)
        //    {
        //        StackPanel row = new StackPanel
        //        {
        //            Orientation = Orientation.Horizontal,
        //            Margin = new Thickness(0, 5, 0, 5)
        //        };

        //        // Gebruikt CategoryNr en CostType als beschrijving
        //        string beschrijving = "Categorie " + cost.Category + " (" + cost.CostType + ")";

        //        row.Children.Add(new TextBlock
        //        {
        //            Text = beschrijving,
        //            Width = 300,
        //            FontWeight = FontWeights.Bold
        //        });

        //        row.Children.Add(new TextBlock
        //        {
        //            Text = cost.Value.ToString("C")
        //        });

        //        panelCosts.Children.Add(row);
        //    }
        //}
        private void LoadFilters()
        {
            List<int> years = repo.GetAvailableYears(_loggedInCompany.Id);
            foreach (int year in years)
            {
                cmbYear.Items.Add(year);
            }

            List<string> types = repo.GetAvailableCostTypes();
            foreach (string type in types)
            {
                cmbCostType.Items.Add(type);
            }
        }
        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (cmbYear.SelectedItem == null || cmbCostType.SelectedItem == null) return;

            int selectedYear = (int)cmbYear.SelectedItem;
            string selectedType = cmbCostType.SelectedItem.ToString();

            List<Cost> kosten = repo.GetFilteredCosts(_loggedInCompany.Id, selectedYear, selectedType);

            panelCosts.Children.Clear();

            decimal totaal = 0;

            foreach (Cost cost in kosten)
            {
                string line = $"{cost.Category} ({cost.CostType})";
                string formatted = line.PadRight(60, '.') + " " + cost.Value.ToString("C");

                panelCosts.Children.Add(new TextBlock
                {
                    Text = formatted,
                    FontSize = 14,
                    FontFamily = new FontFamily("Consolas"),
                    Margin = new Thickness(0, 5, 0, 0)
                });

                totaal += cost.Value;
            }

            // Totaal weergeven
            panelCosts.Children.Add(new TextBlock
            {
                Text = $"TOTAAL{"".PadRight(54, '.')} {totaal.ToString("C")}",
                FontWeight = FontWeights.Bold,
                FontSize = 14,
                FontFamily = new FontFamily("Consolas"),
                Margin = new Thickness(0, 10, 0, 0)
            });
        }
        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CompanyDashboard(_loggedInCompany));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LoginCompany());
        }
    }
}
