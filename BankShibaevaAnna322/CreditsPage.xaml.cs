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
            LoadClients();
            LoadCredits();
        }

        private void LoadClients()
        {
            using (var db = new Entities())
            {
                var clients = db.Clients
                    .Select(c => new
                    {
                        c.ClientID,
                        FullName = c.LastName + " " + c.FirstName + " " + c.Patronymic
                    })
                    .ToList();

                ClientFilterComboBox.ItemsSource = clients;
                ClientFilterComboBox.DisplayMemberPath = "FullName";
                ClientFilterComboBox.SelectedValuePath = "ClientID";
            }
        }

        private void LoadCredits(int? clientId = null, string status = null)
        {
            using (var db = new Entities())
            {
                var creditsQuery = db.Loans.Include("Clients").AsQueryable();

                if (clientId != null)
                {
                    creditsQuery = creditsQuery.Where(c => c.ClientID == clientId);
                }

                if (!string.IsNullOrEmpty(status) && status != "Все")
                {
                    creditsQuery = creditsQuery.Where(c => c.Status == status);
                }

                var credits = creditsQuery
                    .Select(c => new
                    {
                        c.LoanID,
                        ClientFullName = c.Clients.LastName + " " + c.Clients.FirstName + " " + c.Clients.Patronymic,
                        c.LoanType,
                        c.Amount,
                        c.InterestRate,
                        Duration = c.CreditTerm,
                        c.StartDate,
                        Status = c.Status
                    })
                    .ToList();

                CreditsDataGrid.ItemsSource = credits;
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            int? clientId = null;
            string status = null;

            if (ClientFilterComboBox.SelectedItem != null)
            {
                dynamic selectedClient = ClientFilterComboBox.SelectedItem;
                clientId = selectedClient.ClientID;
            }

            if (StatusFilterComboBox.SelectedItem != null)
            {
                status = ((ComboBoxItem)StatusFilterComboBox.SelectedItem).Content.ToString();
            }

            LoadCredits(clientId, status);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ClientFilterComboBox.SelectedItem = null;
            StatusFilterComboBox.SelectedIndex = 0;
            LoadCredits();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditCreditPage());
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (CreditsDataGrid.SelectedItem != null)
            {
                dynamic selectedCredit = CreditsDataGrid.SelectedItem;
                int creditId = selectedCredit.LoanID;
                NavigationService.Navigate(new AddEditCreditPage(creditId));
            }
            else
            {
                MessageBox.Show("Выберите кредит для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CreditsDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить этот кредит?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    dynamic selectedCredit = CreditsDataGrid.SelectedItem;
                    int creditId = selectedCredit.LoanID;

                    using (var db = new Entities())
                    {
                        var credit = db.Loans.Find(creditId);
                        if (credit != null)
                        {
                            // Проверяем, есть ли платежи по кредиту
                            bool hasPayments = db.LoanPayments.Any(p => p.LoanID == creditId);

                            if (hasPayments)
                            {
                                MessageBox.Show("Нельзя удалить кредит с платежами",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }

                            db.Loans.Remove(credit);
                            db.SaveChanges();
                            LoadCredits();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите кредит для удаления", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}