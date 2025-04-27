using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class CreditsPage : Page
    {
        public CreditsPage()
        {
            InitializeComponent();
            LoadCredits();
        }

        private void LoadCredits()
        {
            DataGridLoans.ItemsSource = Entities.GetContext().Credits.ToList();
        }

        private void ButtonAddLoan_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddCreditPage());
        }

        private void ButtonEditLoan_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridLoans.SelectedItem is Credit selectedCredit)
            {
                NavigationService.Navigate(new EditCreditPage(selectedCredit));
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите кредит для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonDelLoan_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridLoans.SelectedItem is Credit selectedCredit)
            {
                NavigationService.Navigate(new DelCreditPage(selectedCredit));
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите кредит для удаления", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

        