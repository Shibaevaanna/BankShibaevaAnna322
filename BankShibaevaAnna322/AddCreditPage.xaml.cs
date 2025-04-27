using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class AddCreditPage : Page
    {
        private Credit _credit = new Credit();

        public AddCreditPage()
        {
            InitializeComponent();
            DataContext = _credit; // Устанавливаем контекст данных для привязки
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();

            // Валидация полей ввода
            if (string.IsNullOrWhiteSpace(_credit.NameOfLoan))
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
                
                _credit.Amount = decimal.Parse(TextBoxAmount.Text);
                _credit.InterestRate = double.Parse(TextBoxInterestRate.Text);
                _credit.Duration = int.Parse(TextBoxDuration.Text);
                _credit.Description = TextBoxDescription.Text;

                Entities.GetContext().Credits.Add(_credit);
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

