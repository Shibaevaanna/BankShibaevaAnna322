using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class OrderCardWindow : Window
    {
        private int _userId;

        public OrderCardWindow(int userId)
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
                    var accounts = db.Accounts
                        .Where(a => a.ClientID == user.Clients.ClientID && a.AccountStatus == "Активен")
                        .ToList();
                    AccountComboBox.ItemsSource = accounts;
                    AccountComboBox.DisplayMemberPath = "AccountNumber";
                    AccountComboBox.SelectedValuePath = "AccountID";
                }
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите счет для привязки карты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CardTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип карты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var account = (Accounts)AccountComboBox.SelectedItem;
            string cardType = ((ComboBoxItem)CardTypeComboBox.SelectedItem).Content.ToString();

            using (var db = new Entities())
            {
                Random random = new Random();
                long cardNumber = 4000000000000000 + random.Next(0, 999999999);

                var newCard = new Cards
                {
                    AccountID = account.AccountID,
                    CardNumber = (int)cardNumber, // Явное приведение типа
                    CardType = cardType,
                    ExpiryDate = DateTime.Now.AddYears(3),
                    CardStatus = "Активна",
                    OwnerFirstName = account.Clients.FirstName,
                    OwnerLastName = account.Clients.LastName
                };

                db.Cards.Add(newCard);
                db.SaveChanges();

                MessageBox.Show($"Карта успешно оформлена. Номер карты: {cardNumber}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}