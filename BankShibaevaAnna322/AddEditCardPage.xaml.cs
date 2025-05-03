using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class AddEditCardPage : Page
    {
        private int? _cardId;
        private Cards _card;

        public string Title => _cardId == null ? "Добавление карты" : "Редактирование карты";

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
                    _card = db.Cards.Include("Accounts").Find(_cardId);
                    if (_card != null)
                    {
                        ClientComboBox.SelectedValue = _card.Accounts.ClientID;
                        LoadAccounts(_card.Accounts.ClientID);
                        AccountComboBox.SelectedValue = _card.AccountID;
                        CardNumberTextBox.Text = _card.CardNumber;
                        CardTypeComboBox.SelectedValue = _card.CardType;
                        ExpiryDatePicker.SelectedDate = _card.ExpiryDate;
                        StatusComboBox.SelectedValue = _card.CardStatus;
                    }
                }
            }
            else
            {
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
                    // Добавление новой карты
                    _card = new Cards
                    {
                        AccountID = (int)AccountComboBox.SelectedValue,
                        CardNumber = CardNumberTextBox.Text,
                        CardType = ((ComboBoxItem)CardTypeComboBox.SelectedItem).Content.ToString(),
                        ExpiryDate = ExpiryDatePicker.SelectedDate,
                        CardStatus = ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString(),
                        OwnerFirstName = ((dynamic)ClientComboBox.SelectedItem).FullName.Split(' ')[1],
                        OwnerLastName = ((dynamic)ClientComboBox.SelectedItem).FullName.Split(' ')[0]
                    };
                    db.Cards.Add(_card);
                }
                else
                {
                    // Редактирование существующей карты
                    _card = db.Cards.Find(_cardId);
                    if (_card != null)
                    {
                        _card.AccountID = (int)AccountComboBox.SelectedValue;
                        _card.CardNumber = CardNumberTextBox.Text;
                        _card.CardType = ((ComboBoxItem)CardTypeComboBox.SelectedItem).Content.ToString();
                        _card.ExpiryDate = ExpiryDatePicker.SelectedDate;
                        _card.CardStatus = ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString();
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Данные карты сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
