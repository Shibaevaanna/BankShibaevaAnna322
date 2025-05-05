using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class EditDepositPage : Page
    {
        private Deposits _deposit; // Изменен тип с Deposit на Deposits

        public EditDepositPage(Deposits deposit) // Изменен параметр
        {
            InitializeComponent();
            _deposit = Entities.GetContext().Deposits.Find(deposit.DepositID);
            if (_deposit == null)
            {
                MessageBox.Show("Вклад не найден");
                NavigationService.GoBack();
                return;
            }
            DataContext = _deposit;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_deposit.NameOfDeposit))
                errors.AppendLine("Введите название вклада");

            if (_deposit.Amount <= 0)
                errors.AppendLine("Введите корректную сумму");

            if (_deposit.InterestRate <= 0)
                errors.AppendLine("Введите корректную процентную ставку");

            if (_deposit.Duration <= 0)
                errors.AppendLine("Введите корректный срок");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Изменения сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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