using System;
using System.Collections.Generic;
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
            this.IsVisibleChanged += CreditsPage_IsVisibleChanged;
        }

        private void CreditsPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                LoadCredits();
            }
        }

        private void LoadCredits()
        {
            var credits = Entities.GetContext().Credits.ToList();

            if (!string.IsNullOrWhiteSpace(SearchLoanName.Text))
            {
                string searchText = SearchLoanName.Text.ToLower();
                credits = credits.Where(c => c.NameOfLoan.ToLower().Contains(searchText)).ToList();
            }

            if (FilterDuration.SelectedIndex > 0)
            {
                string selectedDuration = ((ComboBoxItem)FilterDuration.SelectedItem).Content.ToString();
                // Преобразуем строку с длительностью в число месяцев
                int durationMonths = 0;
                if (selectedDuration.Contains("месяц"))
                {
                    string numberPart = new string(selectedDuration.Where(char.IsDigit).ToArray());
                    int.TryParse(numberPart, out durationMonths);
                }
                credits = credits.Where(c => c.Duration == durationMonths).ToList();
            }

            if (SortBy.SelectedIndex > 0)
            {
                var sortOption = ((ComboBoxItem)SortBy.SelectedItem).Content.ToString();
                switch (sortOption)
                {
                    case "По сумме (по возрастанию)":
                        credits = credits.OrderBy(c => c.Amount).ToList();
                        break;
                    case "По сумме (по убыванию)":
                        credits = credits.OrderByDescending(c => c.Amount).ToList();
                        break;
                    case "По процентной ставке (по возрастанию)":
                        credits = credits.OrderBy(c => c.InterestRate).ToList();
                        break;
                    case "По процентной ставке (по убыванию)":
                        credits = credits.OrderByDescending(c => c.InterestRate).ToList();
                        break;
                }
            }

            DataGridLoans.ItemsSource = credits;
        }

        private void SearchLoanName_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadCredits();
        }

        private void FilterDuration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCredits();
        }

        private void SortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCredits();
        }

        private void ClearFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchLoanName.Text = "";
            FilterDuration.SelectedIndex = 0;
            SortBy.SelectedIndex = 0;
            LoadCredits();
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
