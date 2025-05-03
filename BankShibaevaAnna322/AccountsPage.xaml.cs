using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class AccountsPage : Page
    {
        public AccountsPage()
        {
            InitializeComponent();
            LoadClients();
            LoadAccounts();
        }

        private void LoadClients()
        {
            using (var db = new Entities())
            {
                var clients = db.Clients
                    .Select(c => new
                    {
                        c.ClientID,
                        FullName = c.LastName + " " + c.FirstName + " " + c.Patronymic
                    })
                    .ToList();

                ClientFilterComboBox.ItemsSource = clients;
            }
        }

        private void LoadAccounts(int? clientId = null)
        {
            using (var db = new Entities())
            {
                var accountsQuery = db.Accounts.Include("Clients").AsQueryable();

                if (clientId != null)
                {
                    accountsQuery = accountsQuery.Where(a => a.ClientID == clientId);
                }

                var accounts = accountsQuery
                    .Select(a => new
                    {
                        a.AccountID,
                        a.AccountNumber,
                        ClientFullName = a.Clients.LastName + " " + a.Clients.FirstName + " " + a.Clients.Patronymic,
                        a.AccountType,
                        a.Balance,
                        a.AccountStatus
                    })
                    .ToList();

                AccountsDataGrid.ItemsSource = accounts;
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientFilterComboBox.SelectedItem != null)
            {
                dynamic selectedClient = ClientFilterComboBox.SelectedItem;
                int clientId = selectedClient.ClientID;
                LoadAccounts(clientId);
            }
            else
            {
                LoadAccounts();
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ClientFilterComboBox.SelectedItem = null;
            LoadAccounts();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditAccountPage());
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountsDataGrid.SelectedItem != null)
            {
                dynamic selectedAccount = AccountsDataGrid.SelectedItem;
                int accountId = selectedAccount.AccountID;
                NavigationService.Navigate(new AddEditAccountPage(accountId));
            }
            else
            {
                MessageBox.Show("Выберите счет для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountsDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить этот счет?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    dynamic selectedAccount = AccountsDataGrid.SelectedItem;
                    int accountId = selectedAccount.AccountID;

                    using (var db = new Entities())
                    {
                        var account = db.Accounts.Find(accountId);
                        if (account != null)
                        {
                            // Проверяем, есть ли активные карты или ненулевой баланс
                            bool hasActiveCards = db.Cards.Any(c => c.AccountID == accountId);
                            bool hasNonZeroBalance = account.Balance != 0;

                            if (hasActiveCards || hasNonZeroBalance)
                            {
                                MessageBox.Show("Нельзя удалить счет с активными картами или ненулевым балансом",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }

                            db.Accounts.Remove(account);
                            db.SaveChanges();
                            LoadAccounts();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите счет для удаления", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
