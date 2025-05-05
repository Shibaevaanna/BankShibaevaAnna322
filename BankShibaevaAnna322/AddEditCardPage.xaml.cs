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
                        CardNumberTextBox.Text = _currentCard.CardNumber?.ToString();
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
                        CardNumber = int.Parse(CardNumberTextBox.Text), // Явное приведение
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
                        existingCard.CardNumber = int.Parse(CardNumberTextBox.Text); // Явное приведение
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

        // ... остальные методы без изменений ...
    }
}