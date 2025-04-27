using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace BankShibaevaAnna322
{
    public partial class EditBranchPage : Page
    {
        private Branches branch;

        public EditBranchPage(Branches branch)
        {
            InitializeComponent();
            branch = Entities.GetContext().Branches.Find(branch.Id); // замените Id на имя ключевого поля
            if (branch == null)
            {
                MessageBox.Show("Филиал не найден");
                NavigationService.GoBack();
                return;
            }
            DataContext = branch;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(branch.BranchName))
                errors.AppendLine("Введите название филиала");
            if (string.IsNullOrWhiteSpace(branch.Address))
                errors.AppendLine("Введите адрес");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
