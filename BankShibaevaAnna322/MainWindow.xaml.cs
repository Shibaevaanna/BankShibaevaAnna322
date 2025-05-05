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
            passwordBox = this.FindName("passwordBox") as PasswordBox; // Добавлена инициализация
        }

        private PasswordBox passwordBox; // Добавлено поле

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text.Trim();
            string password = passwordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string hashedPassword = GetHash(password);

            using (var db = new Entities())
            {
                var user = db.Users.Include("Roles").FirstOrDefault(u => u.UserLogin == login && u.UserPassword == hashedPassword);

                if (user == null)
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (user.Roles.RoleName == "Администратор")
                {
                    var adminDashboard = new AdminDashboard();
                    adminDashboard.Show();
                    this.Close();
                }
                else if (user.Roles.RoleName == "Пользователь")
                {
                    var userDashboard = new UserDashboard(user.UserID);
                    userDashboard.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неизвестная роль пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }
    }
}