using System;
using System.Linq;
using System.Windows;

namespace BankShibaevaAnna322
{
    public partial class MakePaymentWindow : Window
    {
        private int _loanId;

        public MakePaymentWindow(int loanId)
        {
            InitializeComponent();
            _loanId = loanId;
            LoadLoanInfo();
        }

        private void LoadLoanInfo()
        {
            using (var db = new Entities())
            {
                var loan = db.Loans.Find(_loanId);
                if (loan != null)
                {
                    LoanIdText.Text = loan.LoanID.ToString();
                }
            }
        }

        private void MakePaymentButton_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(AmountTextBox.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму платежа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new Entities())
            {
                var loan = db.Loans.Find(_loanId);
                if (loan == null)
                {
                    MessageBox.Show("Кредит не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверяем, есть ли у клиента активный счет с достаточным балансом
                var clientAccount = db.Accounts.FirstOrDefault(a => a.ClientID == loan.ClientID && a.AccountStatus == "Активен");
                if (clientAccount == null)
                {
                    MessageBox.Show("У вас нет активного счета для списания средств", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (clientAccount.Balance < amount)
                {
                    MessageBox.Show("Недостаточно средств на счете", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Списание средств со счета
                clientAccount.Balance -= (int)amount;

                // Создаем запись о платеже
                var payment = new LoanPayments
                {
                    LoanID = loan.LoanID,
                    ClientID = loan.ClientID,
                    PaymentAmount = (int)amount,
                    PaymentDate = DateTime.Now
                };

                db.LoanPayments.Add(payment);

                // Проверяем, погашен ли кредит полностью
                var totalPaid = db.LoanPayments.Where(p => p.LoanID == loan.LoanID).Sum(p => p.PaymentAmount);
                if (totalPaid >= loan.Amount)
                {
                    loan.Status = "Погашен";
                }

                db.SaveChanges();

                MessageBox.Show($"Платеж на сумму {amount:C} успешно внесен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
