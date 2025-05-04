using System;
using System.Linq;
using System.Windows;

namespace BankShibaevaAnna322
{
    public partial class TransferWindow : Window
    {
        private int _fromAccountId;

        public TransferWindow(int fromAccountId)
        {
            InitializeComponent();
            _fromAccountId = fromAccountId;
            LoadAccountInfo();
        }

        private void LoadAccountInfo()
        {
            using (var db = new Entities())
            {
                var account = db.Accounts.Find(_fromAccountId);
                if (account != null)
                {
                    FromAccountText.Text = account.AccountNumber.ToString();
                }
            }
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(ToAccountTextBox.Text, out int toAccountNumber))
            {
                MessageBox.Show("Введите корректный номер счета получателя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(AmountTextBox.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму для перевода", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new Entities())
            {
                var fromAccount = db.Accounts.Find(_fromAccountId);
                var toAccount = db.Accounts.FirstOrDefault(a => a.AccountNumber == toAccountNumber);

                if (fromAccount == null || toAccount == null)
                {
                    MessageBox.Show("Один из счетов не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (fromAccount.AccountID == toAccount.AccountID)
                {
                    MessageBox.Show("Нельзя перевести средства на тот же счет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (fromAccount.Balance < amount)
                {
                    MessageBox.Show("Недостаточно средств на счете", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Выполняем перевод
                fromAccount.Balance -= (int)amount;
                toAccount.Balance += (int)amount;

                // Создаем записи о транзакциях
                var fromTransaction = new Transactions
                {
                    AccountID = fromAccount.AccountID,
                    TransactionType = "Перевод (отправка)",
                    Amount = (int)amount,
                    TransactionDate = DateTime.Now,
                    RelatedAccountID = toAccount.AccountID
                };

                var toTransaction = new Transactions
                {
                    AccountID = toAccount.AccountID,
                    TransactionType = "Перевод (получение)",
                    Amount = (int)amount,
                    TransactionDate = DateTime.Now,
                    RelatedAccountID = fromAccount.AccountID
                };

                db.Transactions.Add(fromTransaction);
                db.Transactions.Add(toTransaction);
                db.SaveChanges();

                MessageBox.Show($"Перевод на сумму {amount:C} успешно выполнен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
