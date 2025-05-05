using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class AddCardPage : Page
    {
        private readonly Cards _card = new Cards();

        public AddCardPage()
        {
            InitializeComponent();
            DataContext = _card;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();

            if (_card.CardNumber == null || _card.CardNumber == 0)
                errors.AppendLine("Введите номер карты");

            if (string.IsNullOrWhiteSpace(_card.CardType))
                errors.AppendLine("Введите тип карты");

            if (string.IsNullOrWhiteSpace(_card.CardStatus))
                errors.AppendLine("Введите состояние карты");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Entities.GetContext().Cards.Add(_card);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Новая карта добавлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}