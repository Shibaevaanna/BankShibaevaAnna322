using System;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class DelBranchPage : Page
    {
        private readonly Branches branch;

        public DelBranchPage(Branches branchParam)
        {
            InitializeComponent();
            this.branch = Entities.GetContext().Branches.Find(branchParam.Id);
            if (this.branch == null)
            {
                MessageBox.Show("Филиал не найден");
                NavigationService.GoBack();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.GetContext().Branches.Remove(branch);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Филиал удалён", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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