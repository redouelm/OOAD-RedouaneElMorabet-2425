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
using WpfAdminApp.Pages;

namespace WpfCompanyApp.Pages
{
    /// <summary>
    /// Interaction logic for CompanyDashboard.xaml
    /// </summary>
    /// 
    public partial class CompanyDashboard : Page
    {
        private Company _bedrijf;

        public CompanyDashboard(Company result)
        {
            InitializeComponent();
            _bedrijf = result;
            ToonGegevens();

            txtWelkom.Text = $"Welkom, {_bedrijf.Login}!";
        }

        private void ToonGegevens()
        {
            lblGegevens.Content =
                $"Naam: {_bedrijf.Name}\n" +
                $"Contact: {_bedrijf.Contact}\n" +
                $"Adres: {_bedrijf.Address}, {_bedrijf.Zip} {_bedrijf.City}, {_bedrijf.Country}\n" +
                $"Telefoon: {_bedrijf.Phone}\n" +
                $"Email: {_bedrijf.Email}\n" +
                $"BTW: {_bedrijf.Btw}\n" +
                $"Login: {_bedrijf.Login}\n" +
                $"Status: {_bedrijf.Status}\n" +
                $"Taal: {_bedrijf.Language}";
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LoginCompany());
        }

        private void YearRepportButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new YearReportPage(_bedrijf));
        }

        private void CompanyDashboardButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CompanyDashboard(_bedrijf));
        }
        private void CostOverviewButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CostsPage(_bedrijf));
        }
    }
}
