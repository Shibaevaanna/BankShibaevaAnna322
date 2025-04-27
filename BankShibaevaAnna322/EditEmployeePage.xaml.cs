using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class EditEmployeePage : Page
    {
        private Employees employee; 

        public EditEmployeePage(Employees selectedEmployee)
        {
            InitializeComponent();
            this.employee = Entities.GetContext().Employees.Find(employee.EmployeeId);
            if (this.employee == null)
            {
                MessageBox.Show("Сотрудник не найден");
                NavigationService.GoBack();
                return;
            }
            DataContext = this.employee;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(employee.Name))
                errors.AppendLine("Введите имя сотрудника");

            if (string.IsNullOrWhiteSpace(employee.Position))
                errors.AppendLine("Введите должность");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Изменения сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
