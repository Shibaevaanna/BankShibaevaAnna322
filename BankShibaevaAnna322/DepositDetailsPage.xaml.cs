using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class DepositDetailsPage : Page
    {
        private int _depositId;
        private Deposits _deposit;

        public DepositDetailsPage(int depositId)
        {
            InitializeComponent();
            _depositId = depositId;
            LoadDepositDetails();
        }

        private void LoadDepositDetails()
        {
            using (var db = new Entities())
            {
                _deposit = db.Deposits.Find(_depositId);
                if (_deposit != null)
                {
                    DepositTypeText.Text = _deposit.DepositType;
                    AmountText.Text = _deposit.Amount?.ToString("C");
                    InterestRateText.Text = $"{_deposit.InterestRate}%";
                    DurationText.Text = $"{_deposit.Duration} мес.";
                    StartDateText.Text = _deposit.StartDate?.ToString("dd.MM.yyyy");
                    MaturityDateText.Text = _deposit.MaturityDate?.ToString("dd.MM.yyyy");
                    StatusText.Text = _deposit.Status;
                }
            }
        }

        private void CloseDepositButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите закрыть этот вклад?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var db = new Entities())
                {
                    var deposit = db.Deposits.Find(_depositId);
                    if (deposit != null)
                    {
                        // Находим активный счет клиента для возврата средств
                        var clientAccount = db.Accounts.FirstOrDefault(a => a.ClientID == deposit.ClientID && a.AccountStatus == "Активен");
                        if (clientAccount == null)
                        {
                            MessageBox.Show("Не найден активный счет для возврата средств", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        // Рассчитываем сумму с процентами
                        double monthsPassed = (DateTime.Now - deposit.StartDate).TotalDays / 30;
                        double interestAmount = deposit.Amount * deposit.InterestRate / 100 * monthsPassed / 12;
                        int totalAmount = deposit.Amount + (int)interestAmount;

                        // Возвращаем средства на счет
                        clientAccount.Balance += totalAmount;

                        // Закрываем вклад
                        deposit.Status = "Закрыт";
                        deposit.MaturityDate = DateTime.Now;

                        db.SaveChanges();

                        MessageBox.Show($"Вклад успешно закрыт. На ваш счет возвращено {totalAmount:C} (включая проценты)",
                            "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.GoBack();
                    }
                }
            }
        }
    }
}
