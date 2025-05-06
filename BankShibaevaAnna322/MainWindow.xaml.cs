using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankShibaevaAnna322
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox?.Text?.Trim() ?? string.Empty;
            string password = PasswordBox?.Password ?? string.Empty;

            // Валидация ввода
            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Введите логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                loginTextBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                PasswordBox.Focus();
                return;
            }

            string hashedPassword = GetHash(password);

            try
            {
                using (var db = new Entities())
                {
                    var user = db.Users.Include("Roles")
                                .FirstOrDefault(u => u.UserLogin == login && u.UserPassword == hashedPassword);

                    if (user == null)
                    {
                        MessageBox.Show("Неверный логин или пароль", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (user.Roles == null)
                    {
                        MessageBox.Show("Ошибка: для пользователя не назначена роль",
                                      "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Window newWindow = null;

                    switch (user.Roles.RoleName)
                    {
                        case "Администратор":
                            newWindow = new AdminDashboard();
                            break;
                        case "Пользователь":
                            newWindow = new UserDashboard(user.UserID);
                            break;
                        default:
                            MessageBox.Show("Неизвестная роль пользователя",
                                          "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                    }

                    newWindow.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при авторизации: {ex.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static string GetHash(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }
    }
}