using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class AccountDetailsPage : Page
    {
        private int _accountId;
        private Accounts _account;

        public AccountDetailsPage(int accountId)
        {
            InitializeComponent();
            _accountId = accountId;
            LoadAccountDetails();
        }

        private void LoadAccountDetails()
        {
            using (var db = new Entities())
            {
                _account = db.Accounts.Find(_accountId);
                if (_account != null)
                {
                    AccountNumberText.Text = _account.AccountNumber.ToString();
                    AccountTypeText.Text = _account.AccountType;
                    BalanceText.Text = _account.Balance?.ToString("C");
                    StatusText.Text = _account.AccountStatus;
                }
            }
        }

        private void DepositButton_Click(object sender, RoutedEventArgs e)
        {
            var depositWindow = new DepositWindow(_accountId);
            depositWindow.Owner = Window.GetWindow(this);
            depositWindow.ShowDialog();
            LoadAccountDetails();
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            var transferWindow = new TransferWindow(_accountId);
            transferWindow.Owner = Window.GetWindow(this);
            transferWindow.ShowDialog();
            LoadAccountDetails();
        }

        private void CloseAccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите закрыть этот счет?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var db = new Entities())
                {
                    var account = db.Accounts.Find(_accountId);
                    if (account != null)
                    {
                        if (account.Balance != 0)
                        {
                            MessageBox.Show("Нельзя закрыть счет с ненулевым балансом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        account.AccountStatus = "Закрыт";
                        db.SaveChanges();
                        MessageBox.Show("Счет успешно закрыт", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.GoBack();
                    }
                }
            }
        }
    }
}
