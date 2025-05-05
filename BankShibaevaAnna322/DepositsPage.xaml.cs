using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class DepositsPage : Page
    {
        public DepositsPage()
        {
            InitializeComponent();
            this.IsVisibleChanged += DepositsPage_IsVisibleChanged;
            LoadDeposits();
        }

        private void DepositsPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                LoadDeposits();
            }
        }

        private void LoadDeposits()
        {
            var deposits = Entities.GetContext().Deposits.ToList();

            if (!string.IsNullOrWhiteSpace(SearchDepositName.Text))
            {
                string searchText = SearchDepositName.Text.ToLower();
                deposits = deposits.Where(d => d.NameOfDeposit.ToLower().Contains(searchText)).ToList();
            }

            if (FilterDuration.SelectedIndex > 0)
            {
                string selectedDuration = ((ComboBoxItem)FilterDuration.SelectedItem).Content.ToString();
                int durationMonths = int.Parse(selectedDuration.Split(' ')[0]);
                deposits = deposits.Where(d => d.Duration == durationMonths).ToList();
            }

            if (SortBy.SelectedIndex > 0)
            {
                var sortOption = ((ComboBoxItem)SortBy.SelectedItem).Content.ToString();
                switch (sortOption)
                {
                    case "По сумме (по возрастанию)":
                        deposits = deposits.OrderBy(d => d.Amount).ToList();
                        break;
                    case "По сумме (по убыванию)":
                        deposits = deposits.OrderByDescending(d => d.Amount).ToList();
                        break;
                    case "По процентной ставке (по возрастанию)":
                        deposits = deposits.OrderBy(d => d.InterestRate).ToList();
                        break;
                    case "По процентной ставке (по убыванию)":
                        deposits = deposits.OrderByDescending(d => d.InterestRate).ToList();
                        break;
                }
            }

            DataGridDeposits.ItemsSource = deposits;
        }

        private void SearchDepositName_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadDeposits();
        }

        private void FilterDuration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDeposits();
        }

        private void SortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDeposits();
        }

        private void ClearFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchDepositName.Text = "";
            FilterDuration.SelectedIndex = 0;
            SortBy.SelectedIndex = 0;
            LoadDeposits();
        }

        private void ButtonAddDeposit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddDepositPage());
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridDeposits.SelectedItem is Deposits deposit)
                NavigationService.Navigate(new EditDepositPage(deposit));
            else
                MessageBox.Show("Выберите вклад для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ButtonDelDeposit_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridDeposits.SelectedItem is Deposits deposit)
                NavigationService.Navigate(new DelDepositPage(deposit));
            else
                MessageBox.Show("Выберите вклад для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
