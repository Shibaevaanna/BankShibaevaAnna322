using System;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class EditCreditPage : Page
    {
        private readonly Credit credit;

        public EditCreditPage(Credit credit)
        {
            InitializeComponent();
            credit = Entities.GetContext().Credits.Find(credit.Id);
            if (credit == null)
            {
                MessageBox.Show("Кредит не найден");
                NavigationService.GoBack();
                return;
            }
            LoadData();
        }

        private void LoadData()
        {
            TextBoxName.Text = credit.NameOfLoan;
            TextBoxAmount.Text = credit.Amount.ToString();
            TextBoxInterestRate.Text = credit.InterestRate.ToString();
            TextBoxDuration.Text = credit.Duration.ToString();
            TextBoxDescription.Text = credit.Description;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                credit.NameOfLoan = TextBoxName.Text;
                credit.Amount = decimal.Parse(TextBoxAmount.Text);
                credit.InterestRate = double.Parse(TextBoxInterestRate.Text);
                credit.Duration = int.Parse(TextBoxDuration.Text);
                credit.Description = TextBoxDescription.Text;

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

