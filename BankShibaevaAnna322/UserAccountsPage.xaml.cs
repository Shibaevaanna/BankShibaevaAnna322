using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class UserAccountsPage : Page
    {
        private int _userId;

        public UserAccountsPage(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            using (var db = new Entities())
            {
                var user = db.Users.Include("Clients").FirstOrDefault(u => u.UserID == _userId);
                if (user != null && user.Clients != null)
                {
                    var accounts = db.Accounts.Where(a => a.ClientID == user.Clients.ClientID).ToList();
                    AccountsDataGrid.ItemsSource = accounts;
                }
            }
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountsDataGrid.SelectedItem is Accounts account)
            {
                NavigationService.Navigate(new AccountDetailsPage(account.AccountID));
            }
            else
            {
                MessageBox.Show("Выберите счет для просмотра деталей", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OpenNewAccountButton_Click(object sender, RoutedEventArgs e)
        {
            var openAccountWindow = new OpenAccountWindow(_userId);
            openAccountWindow.Owner = Window.GetWindow(this);
            openAccountWindow.ShowDialog();
            LoadAccounts();
        }
    }
}
