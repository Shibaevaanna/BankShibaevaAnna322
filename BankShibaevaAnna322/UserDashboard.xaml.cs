using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class UserDashboard : Window
    {
        private int _userId;

        public UserDashboard(int userId)
        {
            InitializeComponent();
            _userId = userId;
            MainFrame.Navigate(new UserProfilePage(userId));
        }

        private void ProfileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserProfilePage(_userId));
        }

        private void AccountsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserAccountsPage(_userId));
        }

        private void CardsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserCardsPage(_userId));
        }

        private void CreditsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserCreditsPage(_userId));
        }

        private void DepositsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserDepositsPage(_userId));
        }

        private void TransfersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TransfersPage(_userId));
        }

        private void PaymentsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PaymentsPage(_userId));
        }

        private void LogoutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

