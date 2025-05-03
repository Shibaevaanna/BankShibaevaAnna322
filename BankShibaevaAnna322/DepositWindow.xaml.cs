using System.Linq;
using System.Windows;

namespace BankShibaevaAnna322
{
    public partial class DepositWindow : Window
    {
        private int _accountId;

        public DepositWindow(int accountId)
        {
            InitializeComponent();
            _accountId = accountId;
            LoadAccountInfo();
        }

        private void LoadAccountInfo()
        {
            using (var db = new Entities())
            {
                var account = db.Accounts.Find(_accountId);
                if (account != null)
                {
                    AccountNumberText.Text = account.AccountNumber.ToString();
                }
            }
        }

        private void DepositButton_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(AmountTextBox.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму для пополнения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new Entities())
            {
                var account = db.Accounts.Find(_accountId);
                if (account != null)
                {
                    account.Balance += (int)amount;

                    // Создаем запись о транзакции
                    var transaction = new Transactions
                    {
                        AccountID = account.AccountID,
                        TransactionType = "Пополнение",
                        Amount = (int)amount,
                        TransactionDate = DateTime.Now
                    };

                    db.Transactions.Add(transaction);
                    db.SaveChanges();

                    MessageBox.Show($"Счет успешно пополнен на {amount:C}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
