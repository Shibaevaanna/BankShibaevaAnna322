using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class CardsPage : Page
    {
        public CardsPage()
        {
            InitializeComponent();
            this.IsVisibleChanged += CardsPage_IsVisibleChanged;
            UpdateCards();
        }

        private void CardsPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                UpdateCards();
            }
        }

        private void UpdateCards()
        {
            var cards = Entities.GetContext().Cards.ToList();

            if (!string.IsNullOrWhiteSpace(SearchCardNumber.Text))
            {
                string searchText = SearchCardNumber.Text.ToLower();
                cards = cards.Where(c => c.CardNumber.ToLower().Contains(searchText)).ToList();
            }

            if (FilterCardType.SelectedIndex > 0)
            {
                string selectedType = ((ComboBoxItem)FilterCardType.SelectedItem).Content.ToString();
                cards = cards.Where(c => c.CardType == selectedType).ToList();
            }

            DataGridCards.ItemsSource = cards;
        }

        private void SearchCardNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCards();
        }

        private void FilterCardType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCards();
        }

        private void ClearFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchCardNumber.Text = "";
            FilterCardType.SelectedIndex = 0;
            UpdateCards();
        }

        private void ButtonAddCardOnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddCardPage());
        }

        private void ButtonEditCardOnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridCards.SelectedItem is Cards card)
                NavigationService.Navigate(new EditCardPage(card));
            else
                MessageBox.Show("Выберите карту для редактирования");
        }

        private void ButtonDelCardOnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridCards.SelectedItem is Cards card)
                NavigationService.Navigate(new DelCardPage(card));
            else
                MessageBox.Show("Выберите карту для удаления");
        }
    }
}
