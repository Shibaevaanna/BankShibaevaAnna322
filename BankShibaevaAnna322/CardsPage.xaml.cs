using BankShibaevaAnna322;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BankShibaevaAnna322
{
    /// &lt;summary&gt;
    /// Логика взаимодействия для CardsPage.xaml
    /// &lt;/summary&gt;
    public partial class CardsPage : Page
    {
        public CardsPage()
        {
            InitializeComponent();
            this.IsVisibleChanged += CardsPage_IsVisibleChanged;
            DataGridCards.ItemsSource = Entities.GetContext().Cards.ToList();
        }

        private void CardsPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                DataGridCards.ItemsSource = Entities.GetContext().Cards.ToList();
            }
        }

        private void ButtonAddCard_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddCardPage());
        }

        private void ButtonEditCard_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridCards.SelectedItem is Cards card)
                NavigationService.Navigate(new EditCardPage(card));
            else
                MessageBox.Show("Выберите карту для редактирования");
        }

        private void ButtonDelCard_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridCards.SelectedItem is Cards card)
                NavigationService.Navigate(new DelCardPage(card));
            else
                MessageBox.Show("Выберите карту для удаления");
        }
    }
}
