using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Text.RegularExpressions;

namespace BankShibaevaAnna322
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            string name = nameTextBox.Text.Trim();
            string surname = surnameTextBox.Text.Trim();
            string patronymic = patronymicTextBox.Text.Trim();
            string phone = phoneTextBox.Text.Trim();
            string email = emailTextBox.Text.Trim();
            string login = loginTextBox.Text.Trim();
            string password = passwordBox.Password;
            string confirmPassword = confirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) ||
                string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword) || string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Regex.IsMatch(phone, @"^\+?[0-9]{10,15}$"))
            {
                MessageBox.Show("Введите корректный номер телефона.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Введите корректный email.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать не менее 6 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string hashedPassword = GetHash(password);

            using (var db = new Entities())
            {
                if (db.Users.Any(u => u.UserLogin == login))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (db.Clients.Any(c => c.PhoneNumber == phone))
                {
                    MessageBox.Show("Пользователь с таким номером телефона уже зарегистрирован.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (db.Clients.Any(c => c.Email == email))
                {
                    MessageBox.Show("Пользователь с таким email уже зарегистрирован.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Создаем клиента
                var client = new Clients()
                {
                    FirstName = name,
                    LastName = surname,
                    Patronymic = patronymic,
                    PhoneNumber = phone,
                    Email = email,
                    BirthdayDate = DateTime.Now.AddYears(-18) // По умолчанию 18 лет
                };

                db.Clients.Add(client);
                db.SaveChanges();

                // Получаем роль "Пользователь"
                var userRole = db.Roles.FirstOrDefault(r => r.RoleName == "Пользователь");
                if (userRole == null)
                {
                    userRole = new Roles { RoleName = "Пользователь" };
                    db.Roles.Add(userRole);
                    db.SaveChanges();
                }

                // Создаем пользователя
                var user = new Users()
                {
                    UserLogin = login,
                    UserPassword = hashedPassword,
                    RoleID = userRole.RoleID,
                    UserID = client.ClientID
                };

                db.Users.Add(user);
                db.SaveChanges();

                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Возвращаемся на страницу авторизации
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
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