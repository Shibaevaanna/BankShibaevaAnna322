using System;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class DelCardPage : Page
    {
        private Cards _card;

        public DelCardPage(Cards card)
        {
            InitializeComponent();
            _card = Entities.GetContext().Cards.Find(card.Id);
            if (_card == null)
            {
                MessageBox.Show("Карта не найдена");
                NavigationService.GoBack();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.GetContext().Cards.Remove(_card);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Карта удалена");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}");
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}