using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class CardDetailsPage : Page
    {
        private int _cardId;
        private Cards _card;

        public CardDetailsPage(int cardId)
        {
            InitializeComponent();
            _cardId = cardId;
            LoadCardDetails();
        }

        private void LoadCardDetails()
        {
            using (var db = new Entities())
            {
                _card = db.Cards.Find(_cardId);
                if (_card != null)
                {
                    CardNumberText.Text = _card.CardNumber.ToString(); // Исправлено преобразование int в string
                    CardTypeText.Text = _card.CardType;
                    OwnerText.Text = $"{_card.OwnerLastName} {_card.OwnerFirstName}";
                    ExpiryDateText.Text = _card.ExpiryDate?.ToString("MM/yy");
                    StatusText.Text = _card.CardStatus;
                }
            }
        }

        private void BlockCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите заблокировать эту карту?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var db = new Entities())
                {
                    var card = db.Cards.Find(_cardId);
                    if (card != null)
                    {
                        card.CardStatus = "Заблокирована";
                        db.SaveChanges();
                        MessageBox.Show("Карта успешно заблокирована", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadCardDetails();
                    }
                }
            }
        }

        private void CloseCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите закрыть эту карту?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var db = new Entities())
                {
                    var card = db.Cards.Find(_cardId);
                    if (card != null)
                    {
                        db.Cards.Remove(card);
                        db.SaveChanges();
                        MessageBox.Show("Карта успешно закрыта", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.GoBack();
                    }
                }
            }
        }
    }
}