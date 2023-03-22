using System.Windows;
using System.Windows.Controls;
using TelegramBot.Classes;
using TelegramBot.Classes.Helper;
using TelegramBot.Pages.AdminPanel;

namespace TelegramBot.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageLogin.xaml
    /// </summary>
    public partial class PageLogin : Page
    {
        public PageLogin()
        {
            InitializeComponent();
        }

        private void BtnClickGoPageCreateUser(object sender, RoutedEventArgs e) =>
            FrameNav.FrameNavigation.Navigate(new PageCreateUser());

        private void BtnClickLogin(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)TxbLogin.Template.FindName("TB", TxbLogin);
            var login = textBox.Text;

            textBox = (TextBox)TxbPassword.Template.FindName("TB", TxbPassword);
            var password = textBox.Text;

            var admin = DBAdminHelper.FindAdmin(login, password);
            if (admin == null) return;

            AdminProfileSaver.Admin = admin;
            FrameNav.FrameNavigation.Navigate(new PageMainAdminPanel());
        }
    }
}
