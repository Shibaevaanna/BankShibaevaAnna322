using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class AddEditAccountPage : Page
    {
        private int? _accountId;
        private Accounts _account;

        public string Title => _accountId == null ? "Добавление счета" : "Редактирование счета";

        public AddEditAccountPage(int? accountId = null)
        {
            InitializeComponent();
            _accountId = accountId;
            DataContext = this;
            LoadClients();
            LoadAccountTypes();
            LoadAccountData();
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

                ClientComboBox.ItemsSource = clients;
                ClientComboBox.DisplayMemberPath = "FullName";
                ClientComboBox.SelectedValuePath = "ClientID";
            }
        }

        private void LoadAccountTypes()
        {
            using (var db = new Entities())
            {
                var accountTypes = db.AccountTypes.ToList();
                AccountTypeComboBox.ItemsSource = accountTypes;
                AccountTypeComboBox.DisplayMemberPath = "AccountType";
                AccountTypeComboBox.SelectedValuePath = "AccountTypeID";
            }
        }

        private void LoadAccountData()
        {
            if (_accountId != null)
            {
                using (var db = new Entities())
                {
                    _account = db.Accounts.Find(_accountId);
                    if (_account != null)
                    {
                        ClientComboBox.SelectedValue = _account.ClientID;
                        AccountNumberTextBox.Text = _account.AccountNumber.ToString();
                        AccountTypeComboBox.SelectedValue = db.AccountTypes
                            .FirstOrDefault(at => at.AccountType == _account.AccountType)?.AccountTypeID;
                        BalanceTextBox.Text = _account.Balance.ToString();
                        StatusComboBox.SelectedValue = _account.AccountStatus;
                    }
                }
            }
        }

        private bool ValidateInput()
        {
            if (ClientComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(AccountNumberTextBox.Text) || !int.TryParse(AccountNumberTextBox.Text, out _))
            {
                MessageBox.Show("Введите корректный номер счета", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (AccountTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип счета", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(BalanceTextBox.Text) || !decimal.TryParse(BalanceTextBox.Text, out _))
            {
                MessageBox.Show("Введите корректный баланс", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус счета", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            using (var db = new Entities())
            {
                if (_accountId == null)
                {
                    // Добавление нового счета
                    _account = new Accounts
                    {
                        ClientID = (int)ClientComboBox.SelectedValue,
                        AccountNumber = int.Parse(AccountNumberTextBox.Text),
                        AccountType = ((AccountTypes)AccountTypeComboBox.SelectedItem).AccountType,
                        Balance = decimal.Parse(BalanceTextBox.Text),
                        AccountStatus = ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString()
                    };
                    db.Accounts.Add(_account);
                }
                else
                {
                    // Редактирование существующего счета
                    _account = db.Accounts.Find(_accountId);
                    if (_account != null)
                    {
                        _account.ClientID = (int)ClientComboBox.SelectedValue;
                        _account.AccountNumber = int.Parse(AccountNumberTextBox.Text);
                        _account.AccountType = ((AccountTypes)AccountTypeComboBox.SelectedItem).AccountType;
                        _account.Balance = decimal.Parse(BalanceTextBox.Text);
                        _account.AccountStatus = ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString();
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Данные счета сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
