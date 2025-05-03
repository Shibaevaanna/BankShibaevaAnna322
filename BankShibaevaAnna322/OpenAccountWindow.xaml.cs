using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class OpenAccountWindow : Window
    {
        private int _userId;

        public OpenAccountWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadAccountTypes();
        }

        private void LoadAccountTypes()
        {
            using (var db = new Entities())
            {
                var accountTypes = db.AccountTypes.ToList();
                AccountTypeComboBox.ItemsSource = accountTypes;
                AccountTypeComboBox.DisplayMemberPath = "AccountType";
                AccountTypeComboBox.SelectedValuePath = "AccountTypeID";
            }
        }

        private void OpenAccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип счета", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(InitialDepositTextBox.Text, out decimal initialDeposit) || initialDeposit < 0)
            {
                MessageBox.Show("Введите корректную сумму начального взноса", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new Entities())
            {
                var user = db.Users.Include("Clients").FirstOrDefault(u => u.UserID == _userId);
                if (user == null || user.Clients == null)
                {
                    MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var accountType = (AccountTypes)AccountTypeComboBox.SelectedItem;

                // Генерация номера счета (можно использовать более сложную логику)
                Random random = new Random();
                int accountNumber = random.Next(100000000, 999999999);

                var newAccount = new Accounts
                {
                    ClientID = user.Clients.ClientID,
                    AccountNumber = accountNumber,
                    AccountType = accountType.AccountType,
                    Balance = (int)initialDeposit,
                    AccountStatus = "Активен"
                };

                db.Accounts.Add(newAccount);
                db.SaveChanges();

                MessageBox.Show($"Счет успешно открыт. Номер счета: {accountNumber}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
