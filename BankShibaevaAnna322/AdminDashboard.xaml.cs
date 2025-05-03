using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class AdminDashboard : Window
    {
        public AdminDashboard()
        {
            InitializeComponent();
            MainFrame.Navigate(new ClientsPage());
        }

        private void ClientsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ClientsPage());
        }

        private void AccountsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AccountsPage());
        }

        private void CardsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CardsPage());
        }

        private void CreditsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CreditsPage());
        }

        private void DepositsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DepositsPage());
        }

        private void EmployeesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EmployeesPage());
        }

        private void BranchesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BranchesPage());
        }

        private void LogoutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
