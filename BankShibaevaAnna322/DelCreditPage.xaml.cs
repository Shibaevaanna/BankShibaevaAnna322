using System;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class DelCreditPage : Page
    {
        private readonly Loans _credit; // Изменен тип с Credit на Loans

        public DelCreditPage(Loans credit) // Изменен параметр
        {
            InitializeComponent();
            _credit = Entities.GetContext().Loans.Find(credit.LoanID); // Исправлено на Loans
            if (_credit == null)
            {
                MessageBox.Show("Кредит не найден");
                NavigationService.GoBack();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.GetContext().Loans.Remove(_credit); // Исправлено на Loans
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Кредит удалён", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

