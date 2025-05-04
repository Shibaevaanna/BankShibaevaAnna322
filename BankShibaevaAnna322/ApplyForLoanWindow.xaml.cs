using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows;

namespace BankShibaevaAnna322
{
    public partial class ApplyForLoanWindow : Window
    {
        private int _userId;

        public ApplyForLoanWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
        }

        private void CalculateMonthlyPayment()
        {
            if (!decimal.TryParse(AmountTextBox.Text, out decimal amount) || amount <= 0)
                return;

            if (!int.TryParse(DurationTextBox.Text, out int duration) || duration <= 0)
                return;

            // Упрощенный расчет платежа (аннуитетный)
            double interestRate = 0.12; // 12% годовых
            double monthlyRate = interestRate / 12;
            double coefficient = (monthlyRate * Math.Pow(1 + monthlyRate, duration)) / (Math.Pow(1 + monthlyRate, duration) - 1);
            decimal monthlyPayment = amount * (decimal)coefficient;

            MonthlyPaymentText.Text = monthlyPayment.ToString("C");
        }

        private void AmountTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CalculateMonthlyPayment();
        }

        private void DurationTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CalculateMonthlyPayment();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoanTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип кредита", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(AmountTextBox.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму кредита", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(DurationTextBox.Text, out int duration) || duration <= 0)
            {
                MessageBox.Show("Введите корректный срок кредита", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string loanType = ((ComboBoxItem)LoanTypeComboBox.SelectedItem).Content.ToString();

            using (var db = new Entities())
            {
                var user = db.Users.Include("Clients").FirstOrDefault(u => u.UserID == _userId);
                if (user == null || user.Clients == null)
                {
                    MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newLoan = new Loans
                {
                    ClientID = user.Clients.ClientID,
                    LoanType = loanType,
                    CreditTerm = duration,
                    InterestRate = 12, // 12%
                    StartDate = DateTime.Now,
                    Amount = (int)amount,
                    Status = "Активен"
                };

                db.Loans.Add(newLoan);
                db.SaveChanges();

                MessageBox.Show("Кредит успешно оформлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}