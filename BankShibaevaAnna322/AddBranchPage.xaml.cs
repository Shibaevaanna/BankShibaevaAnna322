using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class AddBranchPage : Page
    {
        private Branches _branch = new Branches();

        public AddBranchPage()
        {
            InitializeComponent();
            DataContext = _branch;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_branch.BranchName))
                errors.AppendLine("Введите название филиала");
            if (string.IsNullOrWhiteSpace(_branch.Address))
                errors.AppendLine("Введите адрес");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Entities.GetContext().Branches.Add(_branch);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Филиал добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
