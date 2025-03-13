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
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Credits_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreditsPage());
        }

        private void Cards_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CardsPage());
        }

        private void Deposits_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DepositsPage());
        }

        private void Employees_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeesPage());
        }
        private void Branches_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BranchesPage());
        }
    }
}
      
 
