using CLBenchmark;
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
using WpfAdminApp.Pages;

namespace WpfCompanyApp.Pages
{
    /// <summary>
    /// Interaction logic for LoginCompany.xaml
    /// </summary>
    public partial class LoginCompany : Page
    {
        public LoginCompany()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = txtEmail.Text;
            string password = pwdPassword.Password;

            CompanyRepository repo = new CompanyRepository();
            Company result = repo.Login(login, password);

            if (result != null)
            {
                NavigationService?.Navigate(new CompanyDashboard(result));
            }
            else
            {
                MessageBox.Show("Login mislukt of status niet actief.");
            }
        }
    }
}
