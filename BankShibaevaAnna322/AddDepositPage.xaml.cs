using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class AddDepositPage : Page
    {
        private Deposit deposit = new Deposit();

        public AddDepositPage()
        {
            InitializeComponent();
            DataContext = deposit;
        }

        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();

            
            if (string.IsNullOrWhiteSpace(deposit.NameOfDeposit))
                errors.AppendLine("Введите название вклада");

            if (deposit.Amount <= 0)
                errors.AppendLine("Введите корректную сумму");

            if (deposit.InterestRate <= 0)
                errors.AppendLine("Введите корректную процентную ставку");

            if (deposit.Duration <= 0)
                errors.AppendLine("Введите корректный срок");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                
                Entities.GetContext().Deposits.Add(deposit);
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