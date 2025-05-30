using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
using LiveCharts;
using LiveCharts.Wpf;

namespace WpfCompanyApp.Pages
{
    public partial class CostComparisonPage : Page
    {
        private Company _loggedInCompany;
        private CostRepository _repo = new CostRepository();

        public SeriesCollection ComparisonSeries { get; set; }
        public string[] Labels { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }
        public Func<double, string> Formatter { get; set; }

        public CostComparisonPage(Company loggedInCompany)
        {
            InitializeComponent();
            _loggedInCompany = loggedInCompany;
            DataContext = this;
            LoadFilters();
        }

        private void LoadFilters()
        {
            var years = _repo.GetAvailableYears(_loggedInCompany.Id);
            foreach (var year in years)
                cmbYear.Items.Add(year);
            cmbYear.SelectedIndex = 0;

            var types = _repo.GetAvailableCostTypes();
            foreach (var type in types)
                cmbType.Items.Add(type);
            cmbType.SelectedIndex = 0;

            List<Company> andereBedrijven = _repo.GetAllOtherCompanies(_loggedInCompany.Id);
            int teller = 1;
            foreach (var bedrijf in andereBedrijven)
            {
                cmbVergelijkBedrijf.Items.Add(new ComboBoxItem
                {
                    Content = $"Bedrijf {teller++}",
                    Tag = bedrijf.Id 
                });
            }
            cmbVergelijkBedrijf.SelectedIndex = 0;
        }

        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbYear.SelectedItem == null || cmbType.SelectedItem == null) return;

            int year = (int)cmbYear.SelectedItem;
            string type = cmbType.SelectedItem.ToString();

            List<CategoryCost> mijnData = _repo.GetCategoryCosts(_loggedInCompany.Id, year, type);

            // optioneel: vergelijkingsbedrijf
            int vergelijkId = ((ComboBoxItem)cmbVergelijkBedrijf.SelectedItem)?.Tag as int? ?? 0;
            List<CategoryCost> andereData = _repo.GetCategoryCosts(vergelijkId, year, type);

            var labels = mijnData.Select(c => c.Category).ToArray();
            var mijnValues = mijnData.Select(c => c.Total).ToArray();
            var andereValues = labels
                .Select(label => andereData.FirstOrDefault(c => c.Category == label)?.Total ?? 0)
                .ToArray();

            ComparisonSeries = new SeriesCollection
    {
        new ColumnSeries { Title = "Jij", Values = new ChartValues<decimal>(mijnValues) },
        new ColumnSeries { Title = "Vergelijk", Values = new ChartValues<decimal>(andereValues) }
    };

            Labels = labels;
            YAxes = new Axis[]
            {
        new Axis { Title = "Kosten (€)", MinValue = 0, LabelFormatter = Formatter }
            };

            XAxes = new Axis[]
            {
        new Axis { Labels = Labels, Title = "Kostenposten" }
            };

            DataContext = null;
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

