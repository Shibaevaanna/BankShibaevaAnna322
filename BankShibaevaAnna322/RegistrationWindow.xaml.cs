using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace BankShibaevaAnna322
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            LoadRoles();
        }

        private void LoadRoles()
        {
            using (var db = new Entities())
            {
                var roles = db.Roles.ToList();
                roleComboBox.ItemsSource = roles;
                if (roles.Any())
                    roleComboBox.SelectedIndex = 0;
            }
        }

        private void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            string name = nameTextBox.Text.Trim();
            string surname = surnameTextBox.Text.Trim();
            string patronymic = patronymicTextBox.Text.Trim();
            string login = loginTextBox.Text.Trim();
            string password = passwordBox.Password;
            string confirmPassword = confirmPasswordBox.Password;
            var selectedRole = roleComboBox.SelectedItem as Roles;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) ||
                string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword) || selectedRole == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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

                var user = new Users()
                {
                    UserLogin = login,
                    UserPassword = hashedPassword,
                    RoleID = selectedRole.RoleID
                };

                db.Users.Add(user);
                db.SaveChanges();
            }

            MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
