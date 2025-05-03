using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BankShibaevaAnna322
{
    public partial class AddEditClientPage : Page
    {
        private int? _clientId;
        private Clients _client;

        public string Title => _clientId == null ? "Добавление клиента" : "Редактирование клиента";

        public AddEditClientPage(int? clientId = null)
        {
            InitializeComponent();
            _clientId = clientId;
            DataContext = this;
            LoadClientData();
        }

        private void LoadClientData()
        {
            if (_clientId != null)
            {
                using (var db = new Entities())
                {
                    _client = db.Clients.Find(_clientId);
                    if (_client != null)
                    {
                        LastNameTextBox.Text = _client.LastName;
                        FirstNameTextBox.Text = _client.FirstName;
                        PatronymicTextBox.Text = _client.Patronymic;
                        BirthDatePicker.SelectedDate = _client.BirthdayDate;
                        PhoneTextBox.Text = _client.PhoneNumber;
                        EmailTextBox.Text = _client.Email;
                    }
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
                MessageBox.Show("Клиент должен быть старше 18 лет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                if (_clientId == null)
                {
                    // Добавление нового клиента
                    _client = new Clients
                    {
                        LastName = LastNameTextBox.Text.Trim(),
                        FirstName = FirstNameTextBox.Text.Trim(),
                        Patronymic = PatronymicTextBox.Text.Trim(),
                        BirthdayDate = BirthDatePicker.SelectedDate,
                        PhoneNumber = PhoneTextBox.Text.Trim(),
                        Email = EmailTextBox.Text.Trim()
                    };
                    db.Clients.Add(_client);
                }
                else
                {
                    // Редактирование существующего клиента
                    _client = db.Clients.Find(_clientId);
                    if (_client != null)
                    {
                        _client.LastName = LastNameTextBox.Text.Trim();
                        _client.FirstName = FirstNameTextBox.Text.Trim();
                        _client.Patronymic = PatronymicTextBox.Text.Trim();
                        _client.BirthdayDate = BirthDatePicker.SelectedDate;
                        _client.PhoneNumber = PhoneTextBox.Text.Trim();
                        _client.Email = EmailTextBox.Text.Trim();
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Данные клиента сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
