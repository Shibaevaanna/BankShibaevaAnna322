using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class AddCreditPage : Page
    {
        private readonly Loans _loan = new Loans(); // Исправлено имя и тип

        public AddCreditPage()
        {
            InitializeComponent();
            DataContext = _loan;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_loan.LoanName))
                errors.AppendLine("Введите название кредита");

            if (string.IsNullOrWhiteSpace(TextBoxAmount.Text) || !decimal.TryParse(TextBoxAmount.Text, out _))
                errors.AppendLine("Введите корректную сумму кредита");

            if (string.IsNullOrWhiteSpace(TextBoxInterestRate.Text) || !double.TryParse(TextBoxInterestRate.Text, out _))
                errors.AppendLine("Введите корректную процентную ставку");

            if (string.IsNullOrWhiteSpace(TextBoxDuration.Text) || !int.TryParse(TextBoxDuration.Text, out _))
                errors.AppendLine("Введите корректный срок кредита");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                _loan.Amount = decimal.Parse(TextBoxAmount.Text);
                _loan.InterestRate = (int)double.Parse(TextBoxInterestRate.Text); // Явное приведение
                _loan.CreditTerm = int.Parse(TextBoxDuration.Text);
                _loan.LoanType = TextBoxDescription.Text;

                Entities.GetContext().Loans.Add(_loan);
                Entities.GetContext().SaveChanges();

                MessageBox.Show("Кредит успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}