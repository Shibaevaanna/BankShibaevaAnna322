using System;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class DelDepositPage : Page
    {
        private Deposits _deposit;

        public DelDepositPage(Deposits deposit)
        {
            InitializeComponent();
            _deposit = Entities.GetContext().Deposits.Find(deposit.DepositID);
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
                var context = Entities.GetContext();
                context.Deposits.Remove(_deposit);
                context.SaveChanges();
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
