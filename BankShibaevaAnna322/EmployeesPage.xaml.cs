using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class EmployeesPage : Page
    {
        public EmployeesPage()
        {
            InitializeComponent();
            this.IsVisibleChanged += EmployeesPageIsVisibleChanged;
            LoadEmployees();
            LoadPositions();
        }

        private void EmployeesPageIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                LoadEmployees();
                LoadPositions();
            }
        }

        private void LoadEmployees()
        {
            var employees = Entities.GetContext().Employees.ToList();

            if (!string.IsNullOrWhiteSpace(SearchEmployeeName.Text))
            {
                string searchText = SearchEmployeeName.Text.ToLower();
                employees = employees.Where(emp => emp.EmployeeName.ToLower().Contains(searchText)).ToList();
            }

            if (FilterPosition.SelectedIndex > 0)
            {
                string selectedPosition = ((ComboBoxItem)FilterPosition.SelectedItem).Content.ToString();
                employees = employees.Where(emp => emp.Position == selectedPosition).ToList();
            }

            DataGridEmployees.ItemsSource = employees;
        }

        private void LoadPositions()
        {
            var positions = Entities.GetContext().Employees.Select(emp => emp.Position).Distinct().OrderBy(pos => pos).ToList();
            FilterPosition.Items.Clear();
            FilterPosition.Items.Add(new ComboBoxItem { Content = "Все", IsSelected = true });
            foreach (var pos in positions)
            {
                FilterPosition.Items.Add(new ComboBoxItem { Content = pos });
            }
        }

        private void SearchEmployeeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadEmployees();
        }

        private void FilterPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadEmployees();
        }

        private void ClearFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchEmployeeName.Text = "";
            FilterPosition.SelectedIndex = 0;
            LoadEmployees();
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
                MessageBox.Show("Выберите сотрудника для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonDelEmployeeOnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridEmployees.SelectedItem is Employees employee)
                NavigationService.Navigate(new DelEmployeePage(employee));
            else
                MessageBox.Show("Выберите сотрудника для удаления", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}