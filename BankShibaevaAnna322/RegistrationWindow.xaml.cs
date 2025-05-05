using System;
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
            // Реализация регистрации
            if (passwordBox.Password != confirmPasswordBox.Password)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }

            using (var db = new Entities())
            {
                var newClient = new Clients
                {
                    FirstName = nameTextBox.Text,
                    LastName = surnameTextBox.Text,
                    Patronymic = patronymicTextBox.Text,
                    PhoneNumber = phoneTextBox.Text,
                    Email = emailTextBox.Text
                };

                db.Clients.Add(newClient);
                db.SaveChanges();

                var newUser = new Users
                {
                    UserLogin = loginTextBox.Text,
                    UserPassword = GetHash(passwordBox.Password),
                    RoleID = 2, // Обычный пользователь
                    ClientID = newClient.ClientID
                };

                db.Users.Add(newUser);
                db.SaveChanges();

                MessageBox.Show("Регистрация успешна");
                this.Close();
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
