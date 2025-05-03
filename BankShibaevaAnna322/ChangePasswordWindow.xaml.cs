using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Linq;

namespace BankShibaevaAnna322
{
    public partial class ChangePasswordWindow : Window
    {
        private int _userId;

        public ChangePasswordWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string currentPassword = CurrentPasswordBox.Password;
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать не менее 6 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Новые пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string currentPasswordHash = GetHash(currentPassword);
            string newPasswordHash = GetHash(newPassword);

            using (var db = new Entities())
            {
                var user = db.Users.Find(_userId);
                if (user == null)
                {
                    MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (user.UserPassword != currentPasswordHash)
                {
                    MessageBox.Show("Текущий пароль неверен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                user.UserPassword = newPasswordHash;
                db.SaveChanges();

                MessageBox.Show("Пароль успешно изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}