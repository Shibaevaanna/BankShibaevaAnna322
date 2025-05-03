using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class ClientsPage : Page
    {
        public ClientsPage()
        {
            InitializeComponent();
            LoadClients();
        }

        private void LoadClients(string searchText = null)
        {
            using (var db = new Entities())
            {
                var clientsQuery = db.Clients.AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    clientsQuery = clientsQuery.Where(c =>
                        c.LastName.Contains(searchText) ||
                        c.FirstName.Contains(searchText) ||
                        c.PhoneNumber.Contains(searchText) ||
                        c.Email.Contains(searchText));
                }

                var clients = clientsQuery
                    .Select(c => new
                    {
                        c.ClientID,
                        FullName = c.LastName + " " + c.FirstName + " " + c.Patronymic,
                        c.PhoneNumber,
                        c.Email,
                        c.BirthdayDate
                    })
                    .ToList();

                ClientsDataGrid.ItemsSource = clients;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            LoadClients(SearchTextBox.Text);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            LoadClients();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditClientPage());
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem != null)
            {
                dynamic selectedClient = ClientsDataGrid.SelectedItem;
                int clientId = selectedClient.ClientID;
                NavigationService.Navigate(new AddEditClientPage(clientId));
            }
            else
            {
                MessageBox.Show("Выберите клиента для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить этого клиента?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    dynamic selectedClient = ClientsDataGrid.SelectedItem;
                    int clientId = selectedClient.ClientID;

                    using (var db = new Entities())
                    {
                        var client = db.Clients.Find(clientId);
                        if (client != null)
                        {
                            // Проверяем, есть ли у клиента активные счета, кредиты или вклады
                            bool hasActiveAccounts = db.Accounts.Any(a => a.ClientID == clientId && a.AccountStatus == "Активен");
                            bool hasActiveLoans = db.Loans.Any(l => l.ClientID == clientId && l.Status == "Активен");
                            bool hasActiveDeposits = db.Deposits.Any(d => d.ClientID == clientId && d.Status == "Активен");

                            if (hasActiveAccounts || hasActiveLoans || hasActiveDeposits)
                            {
                                MessageBox.Show("Нельзя удалить клиента с активными счетами, кредитами или вкладами",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }

                            db.Clients.Remove(client);
                            db.SaveChanges();
                            LoadClients();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента для удаления", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
