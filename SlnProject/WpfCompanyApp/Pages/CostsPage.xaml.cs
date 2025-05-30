using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CLBenchmark;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WPF;

namespace WpfCompanyApp.Pages
{
    public partial class CostsPage : Page
    {
        private Company _loggedInCompany;
        CostRepository repo = new CostRepository();

        public ISeries[] Series { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        public CostsPage(Company loggedInCompany)
        {
            InitializeComponent();
            _loggedInCompany = loggedInCompany;

            LoadFilters();
            LoadChart();
            DataContext = this;
        }

        private void LoadFilters()
        {
            List<int> years = repo.GetAvailableYears(_loggedInCompany.Id);
            cmbYear.Items.Add("-- Alle jaren--");
            foreach (int year in years)
            {
                cmbYear.Items.Add(year);
            }
            cmbYear.SelectedIndex = 0;

            List<string> types = repo.GetAvailableCostTypes();
            cmbCostType.Items.Add("-- Alle  types --");
            foreach (string type in types)
            {
                cmbCostType.Items.Add(type);
            }
            cmbCostType.SelectedIndex = 0;
        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (cmbYear.SelectedItem == null || cmbCostType.SelectedItem == null) return;

            int? selectedYear = null;
            if (!cmbYear.SelectedItem.ToString().Contains("Alle"))
            {
                selectedYear = Convert.ToInt32(cmbYear.SelectedItem);
            }

            string selectedType = cmbCostType.SelectedItem.ToString().Contains("Alle") ? "" : cmbCostType.SelectedItem.ToString();

            List<Cost> kosten = repo.GetFilteredCosts(_loggedInCompany.Id, selectedYear, selectedType);

            panelCosts.Children.Clear();
            decimal totaal = 0;

            foreach (Cost cost in kosten)
            {
                string line = $"{cost.Category}. {cost.CategoryText}";
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

            panelCosts.Children.Add(new TextBlock
            {
                Text = $"TOTAAL{"".PadRight(54, '.')} {totaal.ToString("C")}",
                FontWeight = FontWeights.Bold,
                FontSize = 14,
                FontFamily = new FontFamily("Consolas"),
                Margin = new Thickness(0, 10, 0, 0)
            });

            LoadChart(selectedYear, selectedType);
        }

        private void LoadChart(int? selectedYear = null, string selectedType = "")
        {
            List<YearlyCostSummary> data = repo.GetYearlyCostSummary(_loggedInCompany.Id);

            if (selectedYear != null)
                data = data.Where(d => d.Year == selectedYear).ToList();

            if (!string.IsNullOrEmpty(selectedType))
                data = data.Where(d => d.CostType == selectedType).ToList();

            var grouped = data.GroupBy(d => d.CostType);
            List<ISeries> seriesList = new List<ISeries>();
            List<int> years = data.Select(d => d.Year).Distinct().OrderBy(y => y).ToList();

            foreach (var group in grouped)
            {
                List<double> values = years.Select(year =>
                    (double)group.Where(g => g.Year == year).Sum(g => g.Total)).ToList();

                seriesList.Add(new ColumnSeries<double>
                {
                    Name = group.Key,
                    Values = values
                });
            }

            Series = seriesList.ToArray();

            XAxes = new Axis[]
            {
        new Axis
        {
            Labels = years.Select(y => y.ToString()).ToArray(),
            Name = "Jaar"
        }
            };

            YAxes = new Axis[]
            {
        new Axis
        {
            Name = "Kosten",
            Labeler = value => value.ToString("C")
        }
            };

            DataContext = null; // Force refresh
            DataContext = this;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LoginCompany());
        }

        private void CompanyDashboardButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CompanyDashboard(_loggedInCompany));
        }

        private void YearReportButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new YearReportPage(_loggedInCompany));
        }

        private void CostOverviewButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CostsPage(_loggedInCompany));
        }
        private void CompareCostsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CostComparisonPage(_loggedInCompany));
        }
    }
}
