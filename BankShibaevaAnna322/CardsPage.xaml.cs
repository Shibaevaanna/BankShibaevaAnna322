using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class CardsPage : Page
    {
        public CardsPage()
        {
            InitializeComponent();
            LoadClients();
            LoadAccounts();
            LoadCards();
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
                ClientFilterComboBox.DisplayMemberPath = "FullName";
                ClientFilterComboBox.SelectedValuePath = "ClientID";
            }
        }

        private void LoadAccounts(int? clientId = null)
        {
            using (var db = new Entities())
            {
                var accountsQuery = db.Accounts.AsQueryable();

                if (clientId != null)
                {
                    accountsQuery = accountsQuery.Where(a => a.ClientID == clientId);
                }

                var accounts = accountsQuery
                    .Select(a => new
                    {
                        a.AccountID,
                        a.AccountNumber
                    })
                    .ToList();

                AccountFilterComboBox.ItemsSource = accounts;
                AccountFilterComboBox.DisplayMemberPath = "AccountNumber";
                AccountFilterComboBox.SelectedValuePath = "AccountID";
            }
        }

        private void LoadCards(int? clientId = null, int? accountId = null)
        {
            using (var db = new Entities())
            {
                var cardsQuery = db.Cards.Include("Accounts.Clients").AsQueryable();

                if (clientId != null)
                {
                    cardsQuery = cardsQuery.Where(c => c.Accounts.ClientID == clientId);
                }

                if (accountId != null)
                {
                    cardsQuery = cardsQuery.Where(c => c.AccountID == accountId);
                }

                var cards = cardsQuery
                    .Select(c => new
                    {
                        c.CardID,
                        c.CardNumber,
                        ClientFullName = c.Accounts.Clients.LastName + " " + c.Accounts.Clients.FirstName + " " + c.Accounts.Clients.Patronymic,
                        AccountNumber = c.Accounts.AccountNumber,
                        c.CardType,
                        c.ExpiryDate,
                        c.CardStatus
                    })
                    .ToList();

                CardsDataGrid.ItemsSource = cards;
            }
        }

        private void ClientFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientFilterComboBox.SelectedItem != null)
            {
                dynamic selectedClient = ClientFilterComboBox.SelectedItem;
                int clientId = selectedClient.ClientID;
                LoadAccounts(clientId);
            }
            else
            {
                AccountFilterComboBox.ItemsSource = null;
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            int? clientId = null;
            int? accountId = null;

            if (ClientFilterComboBox.SelectedItem != null)
            {
                dynamic selectedClient = ClientFilterComboBox.SelectedItem;
                clientId = selectedClient.ClientID;
            }

            if (AccountFilterComboBox.SelectedItem != null)
            {
                dynamic selectedAccount = AccountFilterComboBox.SelectedItem;
                accountId = selectedAccount.AccountID;
            }

            LoadCards(clientId, accountId);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ClientFilterComboBox.SelectedItem = null;
            AccountFilterComboBox.SelectedItem = null;
            LoadCards();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditCardPage());
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (CardsDataGrid.SelectedItem != null)
            {
                dynamic selectedCard = CardsDataGrid.SelectedItem;
                int cardId = selectedCard.CardID;
                NavigationService.Navigate(new AddEditCardPage(cardId));
            }
            else
            {
                MessageBox.Show("Выберите карту для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CardsDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить эту карту?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    dynamic selectedCard = CardsDataGrid.SelectedItem;
                    int cardId = selectedCard.CardID;

                    using (var db = new Entities())
                    {
                        var card = db.Cards.Find(cardId);
                        if (card != null)
                        {
                            db.Cards.Remove(card);
                            db.SaveChanges();
                            LoadCards();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите карту для удаления", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
