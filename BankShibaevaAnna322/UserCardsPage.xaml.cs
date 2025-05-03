using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class UserCardsPage : Page
    {
        private int _userId;

        public UserCardsPage(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadCards();
        }

        private void LoadCards()
        {
            using (var db = new Entities())
            {
                var user = db.Users.Include("Clients").FirstOrDefault(u => u.UserID == _userId);
                if (user != null && user.Clients != null)
                {
                    var cards = db.Cards
                        .Where(c => c.Accounts.ClientID == user.Clients.ClientID)
                        .ToList();
                    CardsDataGrid.ItemsSource = cards;
                }
            }
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (CardsDataGrid.SelectedItem is Cards card)
            {
                NavigationService.Navigate(new CardDetailsPage(card.CardID));
            }
            else
            {
                MessageBox.Show("Выберите карту для просмотра деталей", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OrderNewCardButton_Click(object sender, RoutedEventArgs e)
        {
            var orderCardWindow = new OrderCardWindow(_userId);
            orderCardWindow.Owner = Window.GetWindow(this);
            orderCardWindow.ShowDialog();
            LoadCards();
        }
    }
}
