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

namespace WpfAdminApp.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var repo = new AdminRepository();
            bool isValid = repo.Login(txtEmail.Text, pwdPassword.Password);
            if(isValid)
            {
                NavigationService.Navigate(new AdminDashboard());
            }
            else
            {
                MessageBox.Show($"Ongeldige login.");


            }
        }
    }
}
