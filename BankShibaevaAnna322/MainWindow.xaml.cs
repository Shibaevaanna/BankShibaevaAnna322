using System.Linq;
using System.Windows.Controls;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

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
            string login = loginTextBox.Text.Trim();
            string password = passwordBox.Password;
            string selectedRole = ((ComboBoxItem)roleComboBox.SelectedItem).Content.ToString();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string hashedPassword = GetHash(password);

            using (var db = new Entities())
            {
                var user = db.Users.Where(u => u.UserLogin == login && u.UserPassword == hashedPassword).FirstOrDefault();

                if (user == null)
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Получаем роль из навигационного свойства
                string userRole = user.Roles?.RoleName;
                if (userRole != selectedRole)
                {
                    MessageBox.Show("Выбранная роль не соответствует роли пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (userRole == "Администратор")
                {
                    var adminDashboard = new AdminDashboard();
                    adminDashboard.Show();
                    this.Close();
                }
                else if (userRole == "Пользователь")
                {
                    var userDashboard = new UserDashboard();
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

