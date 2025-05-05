using System;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class DelCreditPage : Page
    {
        private readonly Credit credit;

        public DelCreditPage(Credit credit)
        {
            InitializeComponent();
            credit = Entities.GetContext().Loans.Find(credit.Id);
            if (credit == null)
            {
                MessageBox.Show("Кредит не найден");
                NavigationService.GoBack();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.GetContext().Credits.Remove(credit);
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

