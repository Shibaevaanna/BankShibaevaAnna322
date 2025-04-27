using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class BranchesPage : Page
    {
        public BranchesPage()
        {
            InitializeComponent();
            this.IsVisibleChanged += BranchesPage_IsVisibleChanged;
            LoadBranches();
        }

        private void BranchesPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
                LoadBranches();
        }

        private void LoadBranches()
        {
            Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            var branches = Entities.GetContext().Branches.ToList();

            if (!string.IsNullOrWhiteSpace(SearchBranchName.Text))
            {
                string searchName = SearchBranchName.Text.ToLower();
                branches = branches.Where(b => b.BranchName.ToLower().Contains(searchName)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(SearchAddress.Text))
            {
                string searchAddress = SearchAddress.Text.ToLower();
                branches = branches.Where(b => b.Address.ToLower().Contains(searchAddress)).ToList();
            }

            ListViewBranches.ItemsSource = branches;
        }

        private void SearchBranchName_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadBranches();
        }

        private void SearchAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadBranches();
        }

        private void ClearFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchBranchName.Text = "";
            SearchAddress.Text = "";
            LoadBranches();
        }

        private void ButtonAddBranch_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddBranchPage());
        }

        private void ButtonEditBranch_OnClick(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Branches selectedBranch)
                NavigationService.Navigate(new EditBranchPage(selectedBranch));
            else
                MessageBox.Show("Выберите филиал для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonDelBranch_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListViewBranches.SelectedItem is Branches selectedBranch)
                NavigationService.Navigate(new DelBranchPage(selectedBranch));
            else
                MessageBox.Show("Выберите филиал для удаления", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
