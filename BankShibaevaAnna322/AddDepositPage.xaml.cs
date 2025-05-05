using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class AddDepositPage : Page
    {
        private readonly Deposits _deposit = new Deposits(); // Исправлен тип

        public AddDepositPage()
        {
            InitializeComponent();
            DataContext = _deposit;
        }

        private void ButtonSaveClick(object sender, RoutedEventArgs e)
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
                Entities.GetContext().Deposits.Add(_deposit);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Вклад добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancelClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}