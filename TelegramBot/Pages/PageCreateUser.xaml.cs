using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TelegramBot.Classes.Helper;

namespace TelegramBot.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageCreateUser.xaml
    /// </summary>
    public partial class PageCreateUser : Page
    {
        public PageCreateUser()
        {
            InitializeComponent();
        }

        private void BtnClickCreateUser(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)TxbReferalCode.Template.FindName("TB", TxbReferalCode);
            var referalCode = textBox.Text;

            textBox = (TextBox)TxbLogin.Template.FindName("TB", TxbLogin);
            var login = textBox.Text;

            textBox = (TextBox)TxbPassword.Template.FindName("TB", TxbPassword);
            var password = textBox.Text;

            var error = DBAdminHelper.CreateAdmin(referalCode, login, password);
            if(error != null)
            {
                MessageBox.Show(error);
                return;
            }

            FrameNav.FrameNavigation.GoBack();
        }

        private void BtnClickGoBack(object sender, RoutedEventArgs e) =>
            FrameNav.FrameNavigation.GoBack();

    }
}
