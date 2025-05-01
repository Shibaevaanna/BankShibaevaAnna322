using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class EditCardPage : Page
    {
        private Cards _card;

        public EditCardPage(Cards card)
        {
            InitializeComponent();
            _card = Entities.GetContext().Cards.Find(card.CardID);
            if (_card == null)
            {
                MessageBox.Show("Карта не найдена");
                NavigationService.GoBack();
                return;
            }
            DataContext = _card;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_card.CardNumber))
                errors.AppendLine("Введите номер карты");
            if (string.IsNullOrWhiteSpace(_card.CardType))
                errors.AppendLine("Введите тип карты");
            if (string.IsNullOrWhiteSpace(_card.CardStatus))
                errors.AppendLine("Введите состояние карты");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Изменения сохранены");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
