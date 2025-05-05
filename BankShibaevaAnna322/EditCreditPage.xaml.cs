using System;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class EditCreditPage : Page
    {
        private readonly Loans _credit;

        public EditCreditPage(Loans credit) // Изменен тип с Credit на Loans
        {
            InitializeComponent();
            _credit = Entities.GetContext().Loans.Find(credit.LoanID); // Исправлено на Loans
            if (_credit == null)
            {
                MessageBox.Show("Кредит не найден");
                NavigationService.GoBack();
                return;
            }
            LoadData();
        }

        private void LoadData()
        {
            TextBoxName.Text = _credit.LoanName;
            TextBoxAmount.Text = _credit.Amount.ToString();
            TextBoxInterestRate.Text = _credit.InterestRate.ToString();
            TextBoxDuration.Text = _credit.CreditTerm.ToString();
            TextBoxDescription.Text = _credit.LoanType;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _credit.LoanName = TextBoxName.Text;
                _credit.Amount = decimal.Parse(TextBoxAmount.Text);
                _credit.InterestRate = double.Parse(TextBoxInterestRate.Text);
                _credit.CreditTerm = int.Parse(TextBoxDuration.Text);
                _credit.LoanType = TextBoxDescription.Text;

                Entities.GetContext().SaveChanges();

                MessageBox.Show("Данные сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
