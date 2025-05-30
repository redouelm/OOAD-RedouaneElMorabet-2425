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
using System.Xml.Linq;
using CLBenchmark;

namespace WpfAdminApp.Pages
{
    /// <summary>
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Page
    {
        private Dictionary<int, Company> allCompanies = new ();
        private Company selectedCompany;

        public AdminDashboard()
        {
            InitializeComponent();
            lstActive.SelectionChanged += Company_SelectionChanged;
            lstPending.SelectionChanged += Company_SelectionChanged;
            lstRejected.SelectionChanged += Company_SelectionChanged;
            lstSuspended.SelectionChanged += Company_SelectionChanged;

            ReloadCompanies();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadCompanies(); // herlaadt bedrijven telkens wanneer deze pagina wordt getoond
        }
        private void ReloadCompanies()
        {
            lstActive.Items.Clear();
            lstPending.Items.Clear();
            lstRejected.Items.Clear();
            lstSuspended.Items.Clear();
            allCompanies.Clear();

            var repo = new CompanyRepository();
            var companies = repo.GetAllCompanies();

            foreach (var company in companies)
            {
                allCompanies[company.Id] = company;

                switch (company.Status.ToLower())
                {
                    case "active":
                        lstActive.Items.Add(company.Name);
                        break;
                    case "pending":
                        lstPending.Items.Add(company.Name);
                        break;
                    case "rejected":
                        lstRejected.Items.Add(company.Name);
                        break;
                    case "suspended":
                        lstSuspended.Items.Add(company.Name);
                        break;
                }
            }
        }

        // private void Company_SelectionChanged(object sender, SelectionChangedEventArgs e)
        // {
        //    var listBox = sender as ListBox;
        //    if (listBox?.SelectedItem == null) return;
        //    string selectedName = listBox.SelectedItem.ToString();
        //    selectedCompany = allCompanies.Values.FirstOrDefault(c => c.Name == selectedName);
        //    if (selectedCompany == null) return;
        // }
        private void Company_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox == null || listBox.SelectedItem == null) return;

            string selectedName = listBox.SelectedItem.ToString();

            Company gevondenCompany = null;
            foreach (Company c in allCompanies.Values)
            {
                if (c.Name == selectedName)
                {
                    gevondenCompany = c;
                    break;
                }
            }

            if (gevondenCompany == null) return;

            selectedCompany = gevondenCompany;
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LoginPage());
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCompany != null)
            {
                NavigationService?.Navigate(new EditCompanyPage(selectedCompany));
            }
            else
            {
                MessageBox.Show("Selecteer eerst een bedrijf om te bewerken.");
            }
        }
    }
}

