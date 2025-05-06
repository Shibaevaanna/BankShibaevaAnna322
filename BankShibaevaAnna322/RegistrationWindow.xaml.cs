using System;
using System.Data.Entity.Validation;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password != confirmPasswordBox.Password)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }

            // Проверка заполнения обязательных полей
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                string.IsNullOrWhiteSpace(surnameTextBox.Text) ||
                string.IsNullOrWhiteSpace(loginTextBox.Text) ||
                string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                MessageBox.Show("Заполните все обязательные поля");
                return;
            }

            using (var db = new Entities())
            {
                try
                {
                    var newClient = new Clients
                    {
                        FirstName = nameTextBox.Text,
                        LastName = surnameTextBox.Text,
                        Patronymic = string.IsNullOrWhiteSpace(patronymicTextBox.Text) ? null : patronymicTextBox.Text,
                        PhoneNumber = string.IsNullOrWhiteSpace(phoneTextBox.Text) ? null : phoneTextBox.Text,
                        SecondNumber = string.IsNullOrWhiteSpace(secondNumberTextBox.Text) ? null : secondNumberTextBox.Text,
                        Email = string.IsNullOrWhiteSpace(emailTextBox.Text) ? null : emailTextBox.Text
                    };

                    db.Clients.Add(newClient);
                    db.SaveChanges(); // Первое сохранение для получения ClientID

                    var newUser = new Users
                    {
                        UserLogin = loginTextBox.Text,
                        UserPassword = GetHash(passwordBox.Password),
                        RoleID = 2, // Обычный пользователь
                        ClientID = newClient.ClientID
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges(); // Второе сохранение

                    MessageBox.Show("Регистрация успешна");
                    this.Close();
                }
                catch (DbEntityValidationException ex)
                {
                    // Собираем все сообщения об ошибках
                    var errorMessages = new StringBuilder();
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            errorMessages.AppendLine($"Свойство: {validationError.PropertyName} Ошибка: {validationError.ErrorMessage}");
                        }
                    }

                    MessageBox.Show($"Ошибка валидации:\n{errorMessages.ToString()}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string GetHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}