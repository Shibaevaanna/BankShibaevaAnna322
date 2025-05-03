using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class AddEditCreditPage : Page
    {
        private int? _creditId;
        private Loans _credit;

        public string Title => _creditId == null ? "Добавление кредита" : "Редактирование кредита";

        public AddEditCreditPage(int? creditId = null)
        {
            InitializeComponent();
            _creditId = creditId;
            DataContext = this;
            LoadClients();
            LoadCreditData();
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

        private void LoadCreditData()
        {
            if (_creditId != null)
            {
                using (var db = new Entities())
                {
                    _credit = db.Loans.Find(_creditId);
                    if (_credit != null)
                    {
                        ClientComboBox.SelectedValue = _credit.ClientID;
                        LoanTypeComboBox.SelectedValue = _credit.LoanType;
                        AmountTextBox.Text = _credit.Amount.ToString();
                        InterestRateTextBox.Text = _credit.InterestRate.ToString();
                        DurationTextBox.Text = _credit.CreditTerm.ToString();
                        StartDatePicker.SelectedDate = _credit.StartDate;
                        StatusComboBox.SelectedValue = _credit.Status;
                    }
                }
            }
            else
            {
                StartDatePicker.SelectedDate = DateTime.Now;
            }
        }

        private bool ValidateInput()
        {
            if (ClientComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (LoanTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип кредита", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(AmountTextBox.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму кредита", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!double.TryParse(InterestRateTextBox.Text, out double interestRate) || interestRate <= 0)
            {
                MessageBox.Show("Введите корректную процентную ставку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(DurationTextBox.Text, out int duration) || duration <= 0)
            {
                MessageBox.Show("Введите корректный срок кредита", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (StartDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Введите дату начала кредита", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус кредита", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                if (_creditId == null)
                {
                    // Добавление нового кредита
                    _credit = new Loans
                    {
                        ClientID = (int)ClientComboBox.SelectedValue,
                        LoanType = ((ComboBoxItem)LoanTypeComboBox.SelectedItem).Content.ToString(),
                        Amount = decimal.Parse(AmountTextBox.Text),
                        InterestRate = double.Parse(InterestRateTextBox.Text),
                        CreditTerm = int.Parse(DurationTextBox.Text),
                        StartDate = StartDatePicker.SelectedDate.Value,
                        Status = ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString()
                    };
                    db.Loans.Add(_credit);
                }
                else
                {
                    // Редактирование существующего кредита
                    _credit = db.Loans.Find(_creditId);
                    if (_credit != null)
                    {
                        _credit.ClientID = (int)ClientComboBox.SelectedValue;
                        _credit.LoanType = ((ComboBoxItem)LoanTypeComboBox.SelectedItem).Content.ToString();
                        _credit.Amount = decimal.Parse(AmountTextBox.Text);
                        _credit.InterestRate = double.Parse(InterestRateTextBox.Text);
                        _credit.CreditTerm = int.Parse(DurationTextBox.Text);
                        _credit.StartDate = StartDatePicker.SelectedDate.Value;
                        _credit.Status = ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString();
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Данные кредита сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
