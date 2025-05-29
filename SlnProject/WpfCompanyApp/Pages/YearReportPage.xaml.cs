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
    /// Interaction logic for YearReportPage.xaml
    /// </summary>
    public partial class YearReportPage : Page
    {
        private Company _loggedInCompany;

        public YearReportPage(Company loggedInCompany)
        {
            InitializeComponent();
            _loggedInCompany = loggedInCompany;

            LoadYearReports();
        }

        private void LoadYearReports()
        {
            YearReportRepository repo = new YearReportRepository();
            List<Yearreport> reports = repo.GetYearreportsForCompany(_loggedInCompany.Id);

            lstYears.ItemsSource = reports;
        }

        private void LstYears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AnswerRepository repo = new AnswerRepository(); 
            if (lstYears.SelectedItem is Yearreport selected)
            {
                MessageBox.Show($"Geselecteerd jaarrapport ID: {selected.Id}");
                panelAnswers.Children.Clear();

                List<(string QuestionText, string AnswerValue)> data = repo.GetAnswersWithQuestions(selected.Id);

                foreach (var item in data)
                {
                    StackPanel row = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };
                    row.Children.Add(new TextBlock { Text = item.QuestionText + ":", Width = 300, FontWeight = FontWeights.Bold });
                    row.Children.Add(new TextBlock { Text = item.AnswerValue });
                    panelAnswers.Children.Add(row);
                }
            }
        }

        private void CompanyDashboardButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CompanyDashboard(_loggedInCompany));
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LoginCompany());
        }
    }
}

