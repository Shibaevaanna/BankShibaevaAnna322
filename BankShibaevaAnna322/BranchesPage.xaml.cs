using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            ListViewBranches.ItemsSource = Entities.GetContext().Branches.ToList();
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
                MessageBox.Show("Выберите филиал для редактирования");
        }

        private void ButtonDelBranch_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListViewBranches.SelectedItem is Branches selectedBranch)
                NavigationService.Navigate(new DelBranchPage(selectedBranch));
            else
                MessageBox.Show("Выберите филиал для удаления");
        }
    }
}

