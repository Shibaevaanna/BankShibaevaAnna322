using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class AddEmployeePage : Page
    {
        private Employees employee = new Employees();

        public AddEmployeePage()
        {
            InitializeComponent();
            DataContext = employee;
        }

        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(employee.EmployeeName))
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
                Entities.GetContext().Employees.Add(employee);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Новый сотрудник добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancelClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

