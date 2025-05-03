using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class UserCreditsPage : Page
    {
        private int _userId;

        public UserCreditsPage(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadCredits();
        }

        private void LoadCredits()
        {
            using (var db = new Entities())
            {
                var user = db.Users.Include("Clients").FirstOrDefault(u => u.UserID == _userId);
                if (user != null && user.Clients != null)
                {
                    var credits = db.Loans
                        .Where(l => l.ClientID == user.Clients.ClientID)
                        .ToList();
                    CreditsDataGrid.ItemsSource = credits;
                }
            }
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (CreditsDataGrid.SelectedItem is Loans loan)
            {
                NavigationService.Navigate(new CreditDetailsPage(loan.LoanID));
            }
            else
            {
                MessageBox.Show("Выберите кредит для просмотра деталей", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ApplyForLoanButton_Click(object sender, RoutedEventArgs e)
        {
            var applyLoanWindow = new ApplyForLoanWindow(_userId);
            applyLoanWindow.Owner = Window.GetWindow(this);
            applyLoanWindow.ShowDialog();
            LoadCredits();
        }
    }
}