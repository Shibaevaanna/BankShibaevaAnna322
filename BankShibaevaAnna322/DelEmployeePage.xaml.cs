using System;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class DelEmployeePage : Page
    {
        private Employees employee;

        public DelEmployeePage(Employees employee)
        {
            InitializeComponent();
            employee = Entities.GetContext().Employees.Find(employee.EmployeeID);
            if (employee == null)
            {
                MessageBox.Show("Сотрудник не найден");
                NavigationService.GoBack();
            }
        }

        private void ButtonDeleteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.GetContext().Employees.Remove(employee);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Сотрудник удалён", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancelClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
