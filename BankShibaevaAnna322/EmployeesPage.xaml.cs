using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class EmployeesPage : Page
    {
        public EmployeesPage()
        {
            InitializeComponent();
            this.IsVisibleChanged += EmployeesPageIsVisibleChanged;
            DataGridEmployees.ItemsSource = Entities.GetContext().Employees.ToList();
        }

        private void EmployeesPageIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                DataGridEmployees.ItemsSource = Entities.GetContext().Employees.ToList();
            }
        }

        private void ButtonAddEmployeeOnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEmployeePage());
        }

        private void ButtonEditEmployeeOnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridEmployees.SelectedItem is Employees employee)
                NavigationService.Navigate(new EditEmployeePage(employee));
            else
                MessageBox.Show("Выберите сотрудника для редактирования");
        }

        private void ButtonDelEmployeeOnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridEmployees.SelectedItem is Employees employee)
                NavigationService.Navigate(new DelEmployeePage(employee));
            else
                MessageBox.Show("Выберите сотрудника для удаления");
        }
    }
}

