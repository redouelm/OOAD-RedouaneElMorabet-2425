using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using CLBenchmark;
using Microsoft.Win32;

namespace WpfAdminApp.Pages
{
    /// <summary>
    /// Interaction logic for EditCompanyPage.xaml
    /// </summary>
    public partial class EditCompanyPage : Page
    {
        private Company company;
        private byte[]? _logoBytes;
        public EditCompanyPage(Company selected)
        {
            InitializeComponent();

            company = selected;

            txtId.Text = company.Id.ToString();
            txtName.Text = company.Name;
            txtContact.Text = company.Contact;
            txtAddress.Text = company.Address;
            txtZip.Text = company.Zip;
            txtCity.Text = company.City;
            txtCountry.Text = company.Country;
            txtPhone.Text = company.Phone;
            txtEmail.Text = company.Email;
            txtBtw.Text = company.Btw;
            txtLogin.Text = company.Login;
            txtLanguage.Text = company.Language;
            cmbStatus.SelectedItem = cmbStatus.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(i => i.Content.ToString() == company.Status);

            // Toon logo
            if (company.Logo != null && company.Logo.Length > 0)
            {
                var image = new BitmapImage();
                using (var stream = new MemoryStream(company.Logo))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                }
                LogoPreview.Source = image;
            }
            else
            {
                LogoPreview.Source = null; // Geen afbeelding beschikbaar
            }
        }
        private void Save_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            company.Name = txtName.Text;
            company.Contact = txtContact.Text;
            company.Address = txtAddress.Text;
            company.Zip = txtZip.Text;
            company.City = txtCity.Text;
            company.Country = txtCountry.Text;
            company.Phone = txtPhone.Text;
            company.Email = txtEmail.Text;
            company.Btw = txtBtw.Text;
            company.Login = txtLogin.Text;
            company.Language = txtLanguage.Text;
            company.Status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (_logoBytes != null)
            {
                company.Logo = _logoBytes;
            }
            CompanyRepository repo = new CompanyRepository();
            repo.UpdateCompany(company);

            MessageBox.Show("Wijzigingen opgeslagen.");

            // Ga terug naar vorige pagina (AdminDashboard)
            NavigationService.GoBack();
        }
        private void UploadLogo_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Afbeeldingen (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                // Lees afbeelding als byte[]
                _logoBytes = File.ReadAllBytes(filePath);

                // Toon voorbeeld in de UI
                BitmapImage bitmap = new BitmapImage();
                using (var stream = new MemoryStream(_logoBytes))
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                }

                LogoPreview.Source = bitmap;
            }
        }

        private void GoToDashboard_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AdminDashboard());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LoginPage());
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new EditCompanyPage(company));
        }
    }
}
