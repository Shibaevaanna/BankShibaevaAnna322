using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class CreditDetailsPage : Page
    {
        private int _loanId;
        private Loans _loan;

        public CreditDetailsPage(int loanId)
        {
            InitializeComponent();
            _loanId = loanId;
            LoadLoanDetails();
        }

        private void LoadLoanDetails()
        {
            using (var db = new Entities())
            {
                _loan = db.Loans.Find(_loanId);
                if (_loan != null)
                {
                    LoanTypeText.Text = _loan.LoanType;
                    AmountText.Text = _loan.Amount?.ToString("C");
                    InterestRateText.Text = $"{_loan.InterestRate}%";
                    DurationText.Text = $"{_loan.CreditTerm} мес.";
                    StartDateText.Text = _loan.StartDate?.ToString("dd.MM.yyyy");
                    StatusText.Text = _loan.Status;
                }
            }
        }

        private void MakePaymentButton_Click(object sender, RoutedEventArgs e)
        {
            var paymentWindow = new MakePaymentWindow(_loanId);
            paymentWindow.Owner = Window.GetWindow(this);
            paymentWindow.ShowDialog();
            LoadLoanDetails();
        }

        private void EarlyRepaymentButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите погасить кредит досрочно?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var db = new Entities())
                {
                    var loan = db.Loans.Find(_loanId);
                    if (loan != null)
                    {
                        loan.Status = "Погашен";
                        db.SaveChanges();
                        MessageBox.Show("Кредит успешно погашен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadLoanDetails();
                    }
                }
            }
        }
    }
}