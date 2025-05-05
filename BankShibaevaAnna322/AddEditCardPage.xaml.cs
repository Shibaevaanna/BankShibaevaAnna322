using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class AddEditCardPage : Page
    {
        private readonly int? _cardId;
        private Cards _currentCard;

        public new string Title => _cardId == null ? "Добавление карты" : "Редактирование карты";

        public AddEditCardPage(int? cardId = null)
        {
            InitializeComponent();
            _cardId = cardId;
            DataContext = this;
            LoadClients();
            LoadCardData();
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

                ClientComboBox.ItemsSource = clients;
                ClientComboBox.DisplayMemberPath = "FullName";
                ClientComboBox.SelectedValuePath = "ClientID";
            }
        }

        private void LoadAccounts(int clientId)
        {
            using (var db = new Entities())
            {
                var accounts = db.Accounts
                    .Where(a => a.ClientID == clientId && a.AccountStatus == "Активен")
                    .Select(a => new
                    {
                        a.AccountID,
                        a.AccountNumber
                    })
                    .ToList();

                AccountComboBox.ItemsSource = accounts;
                AccountComboBox.DisplayMemberPath = "AccountNumber";
                AccountComboBox.SelectedValuePath = "AccountID";
            }
        }

        private void LoadCardData()
        {
            if (_cardId != null)
            {
                using (var db = new Entities())
                {
                    _currentCard = db.Cards.Include(c => c.Accounts).FirstOrDefault(c => c.CardID == _cardId);
                    if (_currentCard != null)
                    {
                        ClientComboBox.SelectedValue = _currentCard.Accounts?.ClientID;
                        if (_currentCard.Accounts != null)
                        {
                            LoadAccounts(_currentCard.Accounts.ClientID.Value);
                        }
                        AccountComboBox.SelectedValue = _currentCard.AccountID;
                        CardNumberTextBox.Text = _currentCard.CardNumber.ToString();
                        CardTypeComboBox.SelectedValue = _currentCard.CardType;
                        ExpiryDatePicker.SelectedDate = _currentCard.ExpiryDate;
                        StatusComboBox.SelectedValue = _currentCard.CardStatus;
                    }
                }
            }
            else
            {
                _currentCard = new Cards();
                ExpiryDatePicker.SelectedDate = DateTime.Now.AddYears(3);
            }
        }

        private void ClientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientComboBox.SelectedItem != null)
            {
                dynamic selectedClient = ClientComboBox.SelectedItem;
                int clientId = selectedClient.ClientID;
                LoadAccounts(clientId);
            }
            else
            {
                AccountComboBox.ItemsSource = null;
            }
        }

        private bool ValidateInput()
        {
            if (ClientComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (AccountComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите счет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(CardNumberTextBox.Text) || CardNumberTextBox.Text.Length != 16 || !long.TryParse(CardNumberTextBox.Text, out _))
            {
                MessageBox.Show("Введите корректный номер карты (16 цифр)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (CardTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип карты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (ExpiryDatePicker.SelectedDate == null || ExpiryDatePicker.SelectedDate < DateTime.Now)
            {
                MessageBox.Show("Введите корректную дату окончания действия", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус карты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            using (var db = new Entities())
            {
                if (_cardId == null)
                {
                    var newCard = new Cards
                    {
                        AccountID = (int)AccountComboBox.SelectedValue,
                        CardNumber = int.Parse(CardNumberTextBox.Text),
                        CardType = CardTypeComboBox.SelectedValue?.ToString(),
                        ExpiryDate = ExpiryDatePicker.SelectedDate,
                        CardStatus = StatusComboBox.SelectedValue?.ToString(),
                        OwnerFirstName = ((dynamic)ClientComboBox.SelectedItem).FullName.Split(' ')[1],
                        OwnerLastName = ((dynamic)ClientComboBox.SelectedItem).FullName.Split(' ')[0]
                    };
                    db.Cards.Add(newCard);
                }
                else
                {
                    var existingCard = db.Cards.FirstOrDefault(c => c.CardID == _cardId);
                    if (existingCard != null)
                    {
                        existingCard.AccountID = (int)AccountComboBox.SelectedValue;
                        existingCard.CardNumber = int.Parse(CardNumberTextBox.Text);
                        existingCard.CardType = CardTypeComboBox.SelectedValue?.ToString();
                        existingCard.ExpiryDate = ExpiryDatePicker.SelectedDate;
                        existingCard.CardStatus = StatusComboBox.SelectedValue?.ToString();
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Данные карты сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}
