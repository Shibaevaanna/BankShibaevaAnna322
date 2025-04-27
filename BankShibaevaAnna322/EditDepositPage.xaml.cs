using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class EditDepositPage : Page
    {
        private Deposit deposit; // Объявляем переменную deposit

        public EditDepositPage(Deposit deposit)
        {
            InitializeComponent();
            // Ищем вклад по Id
            this.deposit = Entities.GetContext().Deposits.Find(deposit.Id);
            if (this.deposit == null)
            {
                MessageBox.Show("Вклад не найден");
                NavigationService.GoBack();
                return;
            }
            DataContext = this.deposit; // Устанавливаем контекст данных
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            // Используем правильное имя переменной deposit
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
                Entities.GetContext().SaveChanges(); // Сохраняем изменения в базе данных
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