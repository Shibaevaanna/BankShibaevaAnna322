using System;
using System.Linq;
using System.Windows;

namespace BankShibaevaAnna322
{
    public partial class OpenDepositWindow : Window
    {
        private int _userId;

        public OpenDepositWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
            CalculateInterestRate();
        }

        private void CalculateInterestRate()
        {
            if (DepositTypeComboBox.SelectedItem == null || DurationComboBox.SelectedItem == null)
                return;

            string depositType = ((ComboBoxItem)DepositTypeComboBox.SelectedItem).Content.ToString();
            int duration = int.Parse(((ComboBoxItem)DurationComboBox.SelectedItem).Content.ToString());

            double interestRate = 0;

            // Упрощенный расчет ставки в зависимости от типа и срока вклада
            switch (depositType)
            {
                case "Сберегательный":
                    interestRate = 5.0 + duration * 0.1;
                    break;
                case "Накопительный":
                    interestRate = 6.0 + duration * 0.15;
                    break;
                case "Пенсионный":
                    interestRate = 7.0 + duration * 0.2;
                    break;
            }

            InterestRateText.Text = $"{interestRate:F2}%";
        }

        private void DepositTypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CalculateInterestRate();
        }

        private void DurationComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CalculateInterestRate();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if (DepositTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип вклада", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(AmountTextBox.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму вклада", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DurationComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите срок вклада", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string depositType = ((ComboBoxItem)DepositTypeComboBox.SelectedItem).Content.ToString();
            int duration = int.Parse(((ComboBoxItem)DurationComboBox.SelectedItem).Content.ToString());
            double interestRate = double.Parse(InterestRateText.Text.Replace("%", ""));

            using (var db = new Entities())
            {
                var user = db.Users.Include("Clients").FirstOrDefault(u => u.UserID == _userId);
                if (user == null || user.Clients == null)
                {
                    MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверяем, есть ли у клиента активный счет с достаточным балансом
                var clientAccount = db.Accounts.FirstOrDefault(a => a.ClientID == user.Clients.ClientID && a.AccountStatus == "Активен");
                if (clientAccount == null)
                {
                    MessageBox.Show("У вас нет активного счета для открытия вклада", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (clientAccount.Balance < amount)
                {
                    MessageBox.Show("Недостаточно средств на счете", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Списание средств со счета
                clientAccount.Balance -= (int)amount;

                // Создаем вклад
                var newDeposit = new Deposits
                {
                    ClientID = user.Clients.ClientID,
                    DepositType = depositType,
                    Amount = (int)amount,
                    InterestRate = (int)interestRate,
                    Duration = duration,
                    StartDate = DateTime.Now,
                    MaturityDate = DateTime.Now.AddMonths(duration),
                    Status = "Активен"
                };

                db.Deposits.Add(newDeposit);
                db.SaveChanges();

                MessageBox.Show("Вклад успешно открыт", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
