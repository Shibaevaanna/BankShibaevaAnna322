using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class UserDepositsPage : Page
    {
        private int _userId;

        public UserDepositsPage(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadDeposits();
        }

        private void LoadDeposits()
        {
            using (var db = new Entities())
            {
                var user = db.Users.Include("Clients").FirstOrDefault(u => u.UserID == _userId);
                if (user != null && user.Clients != null)
                {
                    var deposits = db.Deposits
                        .Where(d => d.ClientID == user.Clients.ClientID)
                        .ToList();
                    DepositsDataGrid.ItemsSource = deposits;
                }
            }
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (DepositsDataGrid.SelectedItem is Deposits deposit)
            {
                NavigationService.Navigate(new DepositDetailsPage(deposit.DepositID));
            }
            else
            {
                MessageBox.Show("Выберите вклад для просмотра деталей", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OpenNewDepositButton_Click(object sender, RoutedEventArgs e)
        {
            var openDepositWindow = new OpenDepositWindow(_userId);
            openDepositWindow.Owner = Window.GetWindow(this);
            openDepositWindow.ShowDialog();
            LoadDeposits();
        }
    }
}
