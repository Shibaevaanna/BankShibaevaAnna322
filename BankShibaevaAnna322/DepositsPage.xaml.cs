using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class DepositsPage : Page
    {
        public DepositsPage()
        {
            InitializeComponent();
            LoadDeposits();
            this.IsVisibleChanged += DepositsPage_IsVisibleChanged;
        }

        private void DepositsPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
                LoadDeposits();
        }

        private void LoadDeposits()
        {
            DataGridDeposits.ItemsSource = null;
            DataGridDeposits.ItemsSource = Entities.GetContext().Deposits.ToList();
        }

        private void ButtonAddDeposit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddDepositPage());
        }

        private void ButtonEditDeposit_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridDeposits.SelectedItem is Deposit deposit)
                NavigationService.Navigate(new EditDepositPage(deposit));
            else
                MessageBox.Show("Выберите вклад для редактирования");
        }

        private void ButtonDelDeposit_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridDeposits.SelectedItem is Deposit deposit)
                NavigationService.Navigate(new DelDepositPage(deposit));
            else
                MessageBox.Show("Выберите вклад для удаления");
        }
    }
}


