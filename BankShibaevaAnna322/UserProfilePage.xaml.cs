using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Linq;

namespace BankShibaevaAnna322
{
    public partial class UserProfilePage : Page
    {
        private int _userId;
        private Clients _client;

        public UserProfilePage(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadClientData();
        }

        private void LoadClientData()
        {
            using (var db = new Entities())
            {
                var user = db.Users.Include("Clients").FirstOrDefault(u => u.UserID == _userId);
                if (user != null && user.Clients != null)
                {
                    _client = user.Clients;
                    LastNameTextBox.Text = _client.LastName;
                    FirstNameTextBox.Text = _client.FirstName;
                    PatronymicTextBox.Text = _client.Patronymic;
                    BirthDatePicker.SelectedDate = _client.BirthdayDate;
                    PhoneTextBox.Text = _client.PhoneNumber;
                    EmailTextBox.Text = _client.Email;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            using (var db = new Entities())
            {
                var client = db.Clients.Find(_client.ClientID);
                if (client != null)
                {
                    client.LastName = LastNameTextBox.Text.Trim();
                    client.FirstName = FirstNameTextBox.Text.Trim();
                    client.Patronymic = PatronymicTextBox.Text.Trim();
                    client.BirthdayDate = BirthDatePicker.SelectedDate;
                    client.PhoneNumber = PhoneTextBox.Text.Trim();
                    client.Email = EmailTextBox.Text.Trim();

                    db.SaveChanges();
                    MessageBox.Show("Данные успешно сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                BirthDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Заполните все обязательные поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!Regex.IsMatch(PhoneTextBox.Text, @"^\+?[0-9]{10,15}$"))
            {
                MessageBox.Show("Введите корректный номер телефона", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!Regex.IsMatch(EmailTextBox.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Введите корректный email", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (BirthDatePicker.SelectedDate > DateTime.Now.AddYears(-18))
            {
                MessageBox.Show("Вы должны быть старше 18 лет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var changePasswordWindow = new ChangePasswordWindow(_userId);
            changePasswordWindow.Owner = Window.GetWindow(this);
            changePasswordWindow.ShowDialog();
        }
    }
}