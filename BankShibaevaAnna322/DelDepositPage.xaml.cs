using System;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class DelDepositPage : Page
    {
        private Deposit _deposit;

        public DelDepositPage(Deposit deposit)
        {
            InitializeComponent();
            _deposit = Entities.GetContext().Deposits.Find(deposits.Id);
            if (_deposit == null)
            {
                MessageBox.Show("Вклад не найден");
                NavigationService.GoBack();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.GetContext().Deposits.Remove(_deposit);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Вклад удалён", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
