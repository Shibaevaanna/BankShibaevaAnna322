using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class EditBranchPage : Page
    {
        private readonly Branches _branch;

        public EditBranchPage(Branches branch)
        {
            InitializeComponent();

            if (branch == null)
            {
                MessageBox.Show("Филиал не найден");
                NavigationService.GoBack();
                return;
            }

            // Загружаем актуальные данные из БД
            _branch = Entities.GetContext().Branches.Find(branch.Id);

            if (_branch == null)
            {
                MessageBox.Show("Филиал не найден в базе данных");
                NavigationService.GoBack();
                return;
            }

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